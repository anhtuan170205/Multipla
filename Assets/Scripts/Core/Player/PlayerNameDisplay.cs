using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using TMPro;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text displayNameText;
    private void Start()
    {
        HandlePlayerNameChanged(string.Empty, player.PlayerName.Value);
        player.PlayerName.OnValueChanged += HandlePlayerNameChanged;
    }

    private void OnDestroy()
    {
        player.PlayerName.OnValueChanged -= HandlePlayerNameChanged;
    }

    private void HandlePlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        displayNameText.text = newName.ToString();
    }
}
