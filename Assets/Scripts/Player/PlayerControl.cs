using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerControl : MonoBehaviour
{
    private PlayerInput _playerInput;
    private BoxCollider2D _boxCollider2d;
    private Rigidbody2D _playerRb;
    private Animator _playerAnimator;
    private SpriteRenderer _playerSpriteRenderer;

    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private RaycastHit2D _raycastHit;

    private IngredientsScriptableObject _currentIngredient = null;

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

        _playerInput.actions["Move"].performed += ctx =>
        {
            _playerAnimator.SetBool("Moving", true);
            _moveInput = ctx.ReadValue<Vector2>();
            _lastMoveInput = _moveInput;
        };
        _playerInput.actions["Move"].canceled += ctx =>
        {
            _playerAnimator.SetBool("Moving", false);
            _moveInput = ctx.ReadValue<Vector2>();
        };
        _playerInput.actions["Interact"].performed += ctx =>
        {
            Interact();
        };
    }

    private void OnEnable()
    {
        GameController.GameOver.AddListener(GameOver);
    }

    private void OnDisable()
    {
        GameController.GameOver.RemoveListener(GameOver);
    }

    private void Update()
    {
        Move();
        CheckInteract();
    }

    private void Move()
    {
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

    private void Interact()
    {
        if (_raycastHit.collider != null)
        {
            var ingredient = _raycastHit.collider.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                if (CurrentIngredient == null)
                {
                    CurrentIngredient = ingredient.IngredientsScriptableObject;
                    Destroy(ingredient.gameObject);
                } else
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

    private void GameOver()
    {
        _playerInput.actions.FindActionMap("Player").Disable();
    }
}
