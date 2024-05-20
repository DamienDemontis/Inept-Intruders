using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SyncVar]
    public string playerName;

    [SerializeField]
    Behaviour[] componentsToDisable;

    private void Start()
    {
        Debug.Log("isLocalPlayer = " + isLocalPlayer);
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }
}
