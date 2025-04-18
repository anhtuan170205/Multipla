using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class HostGameManager
{
    private Allocation allocation;
    private string joinCode;
    private string lobbyId;
    private const string GameSceneName = "Game";
    private const int MaxConnections = 20;
    public async Task StartHostAsync()
    {
        try 
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
        try 
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
        UnityTransport unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        unityTransport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>
            {
                { "JoinCode", 
                    new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                    )
                }
            };
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("MyLobby", MaxConnections, lobbyOptions);
            lobbyId = lobby.Id;
            HostSingleton.Instance.StartCoroutine(HeartBeatLobby(15f));
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e);
            return;
        }

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }
    public string GetJoinCode()
    {
        return joinCode;
    }

    private IEnumerator HeartBeatLobby(float interval)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(interval);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
