using System;
using Unity.Netcode;
using UnityEngine;

public class LightningTrap : NetworkBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem leftLightningParticles;
    [SerializeField] private ParticleSystem rightLightningParticles;
    [SerializeField] private BoxCollider lightningCollider;
    private bool _isActive = false;

    private void Start()
    {
        Interact();   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();   
        }
    }

    public void Interact()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            leftLightningParticles.Play();
            rightLightningParticles.Play();
        }
        else
        {
            leftLightningParticles.Stop();
            rightLightningParticles.Stop();
        }
        lightningCollider.enabled = _isActive;
        Debug.Log("New lightning trap status:" + _isActive.ToString());
    }

    public string GetId()
    {
        return String.Empty;
    }
}
