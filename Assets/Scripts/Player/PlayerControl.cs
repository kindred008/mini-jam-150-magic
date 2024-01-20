using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerControl : MonoBehaviour
{
    private PlayerInput _playerInput;
    private BoxCollider2D _boxCollider2d;
    private Rigidbody2D _playerRb;

    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;

    private RaycastHit2D _raycastHit;

    [Header("Character Settings")]
    [SerializeField] private float _moveSpeed = 2;
    [SerializeField] private float interactDistance = 0.2f;

    [Header("")]
    [SerializeField] private LayerMask interactLayer;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _playerRb = GetComponent<Rigidbody2D>();

        _playerInput.actions["Move"].performed += ctx =>
        {
            _moveInput = ctx.ReadValue<Vector2>();
            _lastMoveInput = _moveInput;
        };
        _playerInput.actions["Move"].canceled += ctx =>
        {
            _moveInput = ctx.ReadValue<Vector2>();
        };
        _playerInput.actions["Interact"].performed += ctx =>
        {
            Interact();
        };
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
    }

    private void CheckInteract()
    {
        _raycastHit = Physics2D.Raycast(transform.position, _lastMoveInput, interactDistance, interactLayer);
    }

    private void Interact()
    {
        if (_raycastHit.collider != null)
        {
            var interactable = _raycastHit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }

            var witch = _raycastHit.collider.GetComponent<Witch>();
            if (witch != null)
            {
                Debug.Log("Hello witch");
            }
        }
    }
}
