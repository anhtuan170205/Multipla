using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Netcode;

public class LeaderboardEntityDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Color myColor;
    private FixedString32Bytes playerName;

    public ulong ClientId { get; private set; }
    public int Coins { get; private set; }

    public void Initialise(LeaderboardEntityState state)
    {
        ClientId = state.ClientId;
        this.playerName = state.PlayerName;

        if (state.ClientId == NetworkManager.Singleton.LocalClientId)
        {
            displayText.color = myColor;
        }
        UpdateCoins(state.Coins);
    }

    public void UpdateDisplay()
    {
        displayText.text = $"{transform.GetSiblingIndex() + 1}. {playerName}  ({Coins})";
    }

    public void UpdateCoins(int coins)
    {
        Coins = coins;
        UpdateDisplay();
    }
}
