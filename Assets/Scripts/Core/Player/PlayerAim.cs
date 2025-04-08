using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerAim : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretTransform;
    [SerializeField] private InputReader inputReader;


    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        Vector2 aimScreenPosition = inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        turretTransform.up = (aimWorldPosition - (Vector2)turretTransform.position).normalized;
    }
}
