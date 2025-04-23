using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    public static ClientSingleton Instance { get; private set; }

    public ClientGameManager ClientGameManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task<bool> CreateClient()
    {
        ClientGameManager = new ClientGameManager();
        return await ClientGameManager.InitAsync();
    }
    private void OnDestroy()
    {
        ClientGameManager?.Dispose();
    }
}

