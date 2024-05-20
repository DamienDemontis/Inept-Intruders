using UnityEngine;
using Mirror;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    private void Start()
    {
        // You can add any initialization code here for the lobby player
        if (isLocalPlayer)
        {
            CmdSetPlayerName("Player" + netId);
        }
    }

    [Command]
    private void CmdSetPlayerName(string name)
    {
        playerName = name;
    }
}
