using Unity.Netcode;
using UnityEngine;

public class CamGuyPlayerNetworkActivator : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener playerAudioListener;

    public override void OnNetworkSpawn()
    {
        Debug.Log("IsOwner (CamGuyPlayerNetworkActivator.cs) == " + IsOwner);
        if (IsOwner)
        {
            ActivatePlayerCamera();
        }
        else
        {
            DeactivatePlayerCamera();
        }
    }

    private void ActivatePlayerCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = true;
        }

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = true;
        }
    }

    private void DeactivatePlayerCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = false;
        }
    }
}