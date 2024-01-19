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
    private PlayerInput playerInput;
    private BoxCollider2D boxCollider2d;
    private Rigidbody2D playerRb;

    private Vector2 moveInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();

        playerInput.actions["Move"].performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
        playerInput.actions["Move"].canceled += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
    }

    private void Update()
    {
        var moveValue = moveInput * moveSpeed;

        playerRb.velocity = moveValue;
    }
}
