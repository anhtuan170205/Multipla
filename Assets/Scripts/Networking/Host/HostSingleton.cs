using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;

public class HostSingleton : MonoBehaviour
{
    public static HostSingleton Instance { get; private set; }
    public HostGameManager HostGameManager { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            HostGameManager = new HostGameManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        HostGameManager?.Dispose();
    }
}
