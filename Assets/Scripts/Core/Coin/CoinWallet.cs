using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinWallet : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private BountyCoin bountyCoinPrefab;

    [Header("Settings")]
    [SerializeField] private float bountyCoinSpread = 3f;
    [SerializeField] private float bountyPercentageOnDeath = 50f;
    [SerializeField] private int bountyCoinCount = 10;
    [SerializeField] private int minBountyCoinValue = 5;
    [SerializeField] private LayerMask layerMask;
    private float coinRadius;
    private Collider2D[] coinBuffer = new Collider2D[1];

    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        coinRadius = bountyCoinPrefab.GetComponent<CircleCollider2D>().radius;
        health.OnDied += HandleDie;

    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }
        health.OnDied -= HandleDie;
    }

    public void HandleDie(Health health)
    {
        int bountyValue = TotalCoins.Value * Mathf.RoundToInt(bountyPercentageOnDeath / 100f);
        int bountyCoinValue = bountyValue / bountyCoinCount;
        if (bountyCoinValue < minBountyCoinValue)
        {
            return;
        }
        for (int i = 0; i < bountyCoinCount; i++)
        {
            BountyCoin bountyCoinInstance = Instantiate(bountyCoinPrefab, GetSpawnPoint(), Quaternion.identity);
            bountyCoinInstance.SetValue(bountyCoinValue);
            bountyCoinInstance.GetComponent<NetworkObject>().Spawn();
        }
    }

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

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            Vector2 spawnPoint = (Vector2)transform.position + Random.insideUnitCircle * bountyCoinSpread;
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, coinRadius, coinBuffer, layerMask);
            if (numColliders == 0)
            {
                return spawnPoint;
            }
        }
    }
}
