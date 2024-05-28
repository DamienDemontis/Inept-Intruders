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
            CmdSetPlayerName("Player" + NetworkServer.connections.Count); // Assign player name based on the join order
        }
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
        if (isServer)
        {
            LobbyManager.Instance.UpdatePlayerList();
        }
    }

    [Command]
    public void CmdSelectCharacter(string character)
    {
        if (character == "Cam Guy" && IsCamGuyTaken())
        {
            // If "Cam Guy" is already taken, don't allow selection
            return;
        }

        selectedCharacter = character;
        if (isServer)
        {
            LobbyManager.Instance.UpdatePlayerList();
        }
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
        if (isClient)
        {
            LobbyManager.Instance.UpdatePlayerList();
        }
    }
}
