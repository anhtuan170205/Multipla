using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class HealingZone : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Image healPowerBar;

    [Header("Settings")]
    [SerializeField] private int maxHealPower = 30;
    [SerializeField] private float healCooldown = 10f;
    [SerializeField] private float healTickRate = 1f;
    [SerializeField] private int coinPerTick = 10;
    [SerializeField] private int healPerTick = 10;
    private float remainingHealCooldown;
    private float tickTimer;

    List<Player> playersInZone = new List<Player>();
    private NetworkVariable<int> HealPower = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            HealPower.OnValueChanged += HandleHealPowerChanged;
            HandleHealPowerChanged(0, HealPower.Value);
        }

        if (IsServer)
        {
            HealPower.Value = maxHealPower;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            HealPower.OnValueChanged -= HandleHealPowerChanged;
        }
    }

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
    }

    private void HandleHealPowerChanged(int oldHealPower, int newHealPower)
    {
        healPowerBar.fillAmount = newHealPower / (float)maxHealPower;
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (remainingHealCooldown > 0f)
        {
            remainingHealCooldown -= Time.deltaTime;
            if (remainingHealCooldown <= 0f)
            {
                HealPower.Value = maxHealPower;
            }
            else
            {
                return;
            }
        }
        tickTimer += Time.deltaTime;
        if (tickTimer >= 1 / healTickRate)
        {
            foreach (Player player in playersInZone)
            {
                if (HealPower.Value == 0)
                {
                    break;
                }
                if (player.Health.CurrentHealth.Value == player.Health.MaxHealth)
                {
                    continue;
                }
                if (player.Wallet.TotalCoins.Value < coinPerTick)
                {
                    continue;
                }
                player.Wallet.SpendCoins(coinPerTick);
                player.Health.RestoreHealth(healPerTick);
                HealPower.Value -= 1;
                if (HealPower.Value == 0)
                {
                    remainingHealCooldown = healCooldown;
                }
            }
            tickTimer = tickTimer % (1 / healTickRate);
        }
    }
}
