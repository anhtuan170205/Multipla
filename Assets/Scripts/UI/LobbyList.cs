using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyList : MonoBehaviour
{
    [SerializeField] private Transform lobbyItemContainer;
    [SerializeField] private LobbyItem lobbyItemPrefab;
    private bool isJoining;
    private bool isRefreshing;
    private void OnEnable()
    {
        RefreshLobbies();
    }

    public async void RefreshLobbies()
    {
        if (isRefreshing)
        {
            return;
        }
        isRefreshing = true;
        try 
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions();
            queryLobbiesOptions.Count = 25;
            queryLobbiesOptions.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"),
            };
            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions); 
            foreach (Transform child in lobbyItemContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
                LobbyItem lobbyItem = Instantiate(lobbyItemPrefab, lobbyItemContainer);
                lobbyItem.Initialize(this, lobby);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
        }
        
        isRefreshing = false;
    }

    public async void JoinAsync(Lobby lobby)
    {
        if (isJoining)
        {
            return;
        }
        isJoining = true;
        try
        {
            Lobby joinLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joinLobby.Data["JoinCode"].Value;
            await ClientSingleton.Instance.ClientGameManager.StartClientAsync(joinCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            return;
        }
        isJoining = false;
    }
}
