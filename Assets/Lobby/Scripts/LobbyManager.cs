using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayerList(List<string> playerNames, List<string> selectedCharacters)
    {
        var mainMenuManager = FindObjectOfType<MainMenuManager>();
        if (mainMenuManager != null)
        {
            mainMenuManager.UpdatePlayerList(playerNames, selectedCharacters);
        }
    }

    [Server]
    public void UpdatePlayerList()
    {
        var players = FindObjectsOfType<LobbyPlayer>();
        var playerNames = new List<string>();
        var selectedCharacters = new List<string>();

        foreach (var player in players)
        {
            playerNames.Add(player.playerName);
            selectedCharacters.Add(player.selectedCharacter);
            Debug.Log($"Player: {player.playerName}, Character: {player.selectedCharacter}");
        }

        RpcUpdatePlayerList(playerNames, selectedCharacters);
    }

    [Server]
    public void AddPlayer(LobbyPlayer player)
    {
        UpdatePlayerList();
    }
}
