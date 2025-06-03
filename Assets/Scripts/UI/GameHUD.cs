using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameHUD : MonoBehaviour
{
    public void LeaveGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.HostGameManager.Shutdown();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            ClientSingleton.Instance.ClientGameManager.Disconnect();
        }
    }
}
