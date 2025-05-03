using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class Player : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Header("Settings")]
    [SerializeField] private int cameraPriority = 15;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            virtualCamera.Priority = cameraPriority;
        }
    }
}
