using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private float coinPercentageOnDeath = 0.5f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        Player[] players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (Player player in players)
        {
            HandlePlayerSpawned(player);
        }

        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(Player player)
    {
        player.Health.OnDied += (health) => HandlePlayerDied(player);
    }

    private void HandlePlayerDespawned(Player player)
    {
        player.Health.OnDied -= (health) => HandlePlayerDied(player);
    }

    private void HandlePlayerDied(Player player)
    {
        int coinsOnDeath = Mathf.RoundToInt(player.Wallet.TotalCoins.Value * coinPercentageOnDeath / 100f);
        Destroy(player.gameObject);
        StartCoroutine(RespawnPlayer(player.OwnerClientId, coinsOnDeath));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int coinsOnDeath)
    {
        yield return null;
        Player playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.Wallet.TotalCoins.Value += coinsOnDeath;
    }
}
