using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerControl : MonoBehaviour
{
    private PlayerInput _playerInput;
    private BoxCollider2D _boxCollider2d;
    private Rigidbody2D _playerRb;
    private Animator _playerAnimator;
    private SpriteRenderer _playerSpriteRenderer;
    private Light2D _playerLight;
    private AudioSource _playerAudioSource;

    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private RaycastHit2D _raycastHit;
    private bool _isPaused;

    private IngredientsScriptableObject _currentIngredient = null;

    [Header("References")]
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _pauseMenuButtonUI;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _itemPickupClip;

    public IngredientsScriptableObject CurrentIngredient
    {
        get => _currentIngredient;
        private set
        {
            _currentIngredient = value;

            if (value != null)
            {
                _currentIngredientUIImage.sprite = value.Graphic;
                _currentIngredientUIImage.enabled = true;
            } else
            {
                _currentIngredientUIImage.enabled = false;
            }
        }
    }

    [Header("Character Settings")]
    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private float _interactDistance = 0.2f;

    [Header("References")]
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private Image _currentIngredientUIImage;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerLight = GetComponent<Light2D>();
        _playerAudioSource = GetComponent<AudioSource>();

        _playerAudioSource.volume = GlobalData.EffectsVolume == 0 ? GlobalData.EffectsVolume : GlobalData.EffectsVolume / 4;
    }

    private void OnEnable()
    {
        GameController.GameOver.AddListener(HandleGameOver);

        _playerInput.actions["Move"].performed += MovePerformed;
        _playerInput.actions["Move"].canceled += MoveCanceled;
        _playerInput.actions["Interact"].performed += InteractPerformed;
        _playerInput.actions["Pause"].performed += PausePerformed;
    }

    private void OnDisable()
    {
        GameController.GameOver.RemoveListener(HandleGameOver);

        _playerInput.actions["Move"].performed -= MovePerformed;
        _playerInput.actions["Move"].canceled -= MoveCanceled;
        _playerInput.actions["Interact"].performed -= InteractPerformed;
        _playerInput.actions["Pause"].performed -= PausePerformed;
    }

    private void Update()
    {
        Move();
        CheckInteract();
    }

    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        _playerAnimator.SetBool("Moving", true);
        _moveInput = ctx.ReadValue<Vector2>();
        _lastMoveInput = _moveInput;
    }

    private void MoveCanceled(InputAction.CallbackContext ctx)
    {
        _playerAnimator.SetBool("Moving", false);
        _moveInput = ctx.ReadValue<Vector2>();
    }

    private void InteractPerformed(InputAction.CallbackContext ctx)
    {
        if (_isPaused)
            return;

        if (_raycastHit.collider != null)
        {
            var ingredient = _raycastHit.collider.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                GameController.PlayClip.Invoke(_itemPickupClip);
                if (CurrentIngredient == null)
                {
                    CurrentIngredient = ingredient.IngredientsScriptableObject;
                    Destroy(ingredient.gameObject);
                }
                else
                {
                    var newIngredient = ingredient.IngredientsScriptableObject;
                    ingredient.ChangeIngredient(CurrentIngredient);

                    CurrentIngredient = newIngredient;
                }
            }

            var witch = _raycastHit.collider.GetComponentInParent<Witch>();
            if (witch != null)
            {
                if (witch.HandIngredient(CurrentIngredient))
                {
                    CurrentIngredient = null;
                }
            }
        }
    }

    private void PausePerformed(InputAction.CallbackContext ctx)
    {
        if (_isPaused)
        {
            _playerLight.enabled = true;
            _isPaused = false;
            GameController.Pause.Invoke(false);
        } else
        {
            _playerLight.enabled = false;
            _isPaused = true;
            GameController.Pause.Invoke(true);
        }
    }

    private void Move()
    {
        if (_isPaused)
            return;

        var moveValue = _moveInput * _moveSpeed;
        _playerRb.velocity = moveValue;

        if (moveValue.x > 0)
        {
            _playerSpriteRenderer.flipX = true;
        } else if (moveValue.x < 0)
        {
            _playerSpriteRenderer.flipX = false;
        }
    }

    private void CheckInteract()
    {
        // bool isDiagonal = Mathf.Abs(_lastMoveInput.x) > 0 && Mathf.Abs(_lastMoveInput.y) > 0;

        var interactDistanceFromCenter = (_boxCollider2d.size.x / 2) + _interactDistance;

        _raycastHit = Physics2D.Raycast(transform.position, _lastMoveInput, interactDistanceFromCenter, _interactLayer);
    }

    private void HandleGameOver()
    {
        _playerAudioSource.Stop();
        _playerInput.actions.FindActionMap("Player").Disable();
        _playerLight.enabled = false;
        CurrentIngredient = null;
    }
}
