using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class RobertPlayerNetworkActivator : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
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
        if (playerVirtualCamera != null)
        {
            playerVirtualCamera.enabled = true;
        }

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = true;
        }
    }

    private void DeactivatePlayerCamera()
    {
        if (playerVirtualCamera != null)
        {
            playerVirtualCamera.enabled = false;
        }

        if (playerAudioListener != null)
        {
            playerAudioListener.enabled = false;
        }
    }
}