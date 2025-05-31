using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class HealingZone : NetworkBehaviour
{
    [Header("References")]
    // [SerializeField] private Image healPowerBar;

    [Header("Settings")]
    // [SerializeField] private int maxHealPower = 30;
    // [SerializeField] private float healCooldown = 60f;
    // [SerializeField] private float healTickRate = 1f;
    // [SerializeField] private int coinPerTick = 10;
    // [SerializeField] private int healPerTick = 10;

    List<Player> playersInZone = new List<Player>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer)
        {
            return;
        }
        if (!other.attachedRigidbody.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        playersInZone.Add(player);
        Debug.Log($"Player {player.PlayerName.Value} entered healing zone.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsServer)
        {
            return;
        }
        if (!other.attachedRigidbody.TryGetComponent<Player>(out Player player))
        {
            return;
        }
        playersInZone.Remove(player);
        Debug.Log($"Player {player.name} exited healing zone.");
    }
}
