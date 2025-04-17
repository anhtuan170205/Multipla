using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
    public async void StartHost()
    {
        await HostSingleton.Instance.HostGameManager.StartHostAsync();
    }
    public async void StartClient()
    {
        await ClientSingleton.Instance.ClientGameManager.StartClientAsync(joinCodeField.text);
    }
}
