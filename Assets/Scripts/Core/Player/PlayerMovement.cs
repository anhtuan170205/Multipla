using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f; // degrees per second

    private Vector2 previousMoveInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        inputReader.MoveEvent -= HandleMove;
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        rb.velocity = previousMoveInput * moveSpeed;

        if (previousMoveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(previousMoveInput.y, previousMoveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, targetRotation, rotationSpeed);
        }
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput.normalized;
    }
}
