using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Health : NetworkBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    private bool isDead;
    public Action<Health> OnDied;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void RestoreHealth(int amount)
    {
        ModifyHealth(amount);
    }

    private void ModifyHealth(int amount)
    {
        if (isDead)
        {
            return;
        }
        int newHealth = CurrentHealth.Value + amount;
        CurrentHealth.Value = Mathf.Clamp(newHealth, 0, MaxHealth);
        if (CurrentHealth.Value <= 0)
        {
            isDead = true;
            OnDied?.Invoke(this);
        }
    }
}
