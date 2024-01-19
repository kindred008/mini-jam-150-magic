using System.Collections;
using System.Collections.Generic;
using System.Transactions;
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

    private Vector2 moveInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _playerRb = GetComponent<Rigidbody2D>();

        _playerInput.actions["Move"].performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
        _playerInput.actions["Move"].canceled += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
    }

    private void Update()
    {
        var moveValue = moveInput * moveSpeed;

        _playerRb.velocity = moveValue;
    }
}
