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
    [SerializeField] private float turnRate = 30f;

    private Vector2 previousMoveInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent += HandleMove;

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        float zRotation =  previousMoveInput.x * -turnRate * Time.deltaTime * Mathf.Sign(previousMoveInput.y);
        bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        rb.velocity = (Vector2)bodyTransform.up * previousMoveInput.y * moveSpeed;
    }

    private void HandleMove(Vector2 moveInput)
    {
        previousMoveInput = moveInput;
    }
}
