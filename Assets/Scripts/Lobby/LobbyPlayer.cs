using UnityEngine;
using Mirror;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    private void Start()
    {
        if (isLocalPlayer)
        {
            CmdSetPlayerName("Player" + (NetworkServer.connections.Count)); // Assign player name based on the join order
        }
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
        CustomNetworkManager.Instance.UpdatePlayerList();
    }
}
