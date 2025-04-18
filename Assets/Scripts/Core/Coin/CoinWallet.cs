using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Coin>(out Coin coin))
        {
            return;
        }
        int coinValue = coin.Collect();
        if (!IsServer)
        {
            return;
        }
        TotalCoins.Value += coinValue;
    }

    public void SpendCoins(int amount)
    {
        TotalCoins.Value -= amount;
    }
}
