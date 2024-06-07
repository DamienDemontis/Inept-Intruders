using UnityEngine;
using Mirror;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    [SyncVar(hook = nameof(OnCharacterChanged))]
    public string selectedCharacter = "Robert"; // Default character

    private void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName();
        }
    }

    [Command]
    private void CmdSetPlayerName()
    {
        playerName = $"Player{CustomNetworkManager.Instance.GetNextPlayerNumber()}";
        LobbyManager.Instance.UpdatePlayerList();
    }

    [Command]
    public void CmdSelectCharacter(string character)
    {
        Debug.Log($"CmdSelectCharacter called for player {playerName} to select {character}");

        if (character == "Cam Guy" && IsCamGuyTaken())
        {
            Debug.Log("Cam Guy is already taken.");
            return;
        }

        selectedCharacter = character;
        LobbyManager.Instance.UpdatePlayerList();
    }

    private bool IsCamGuyTaken()
    {
        var players = FindObjectsOfType<LobbyPlayer>();
        foreach (var player in players)
        {
            if (player.selectedCharacter == "Cam Guy")
            {
                return true;
            }
        }
        return false;
    }

    private void OnCharacterChanged(string oldCharacter, string newCharacter)
    {
        Debug.Log($"Character changed for player {playerName} from {oldCharacter} to {newCharacter}");
        if (isClient)
        {
            LobbyManager.Instance.UpdatePlayerList();
        }
    }
}
