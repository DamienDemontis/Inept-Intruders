using System;
using Unity.Netcode;
using UnityEngine;

public class LightningTrap : NetworkBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem leftLightningParticles;
    [SerializeField] private ParticleSystem rightLightningParticles;
    [SerializeField] private BoxCollider lightningCollider;
    private NetworkVariable<bool> _isActive = new NetworkVariable<bool>();

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
        if (IsServer)
        {
            UpdateTrapStatus(!_isActive.Value);
        }
        else
        {
            RequestTrapStatusChangeServerRpc();
        }
    }

    [ServerRpc]
    private void RequestTrapStatusChangeServerRpc(ServerRpcParams rpcParams = default)
    {
        UpdateTrapStatus(!_isActive.Value);
    }

    private void UpdateTrapStatus(bool newStatus)
    {
        _isActive.Value = newStatus;
        UpdateTrapEffects(newStatus);
    }

    private void UpdateTrapEffects(bool isActive)
    {
        lightningCollider.enabled = isActive;
        if (isActive)
        {
            leftLightningParticles.Play();
            rightLightningParticles.Play();
        }
        else
        {
            leftLightningParticles.Stop();
            rightLightningParticles.Stop();
        }
        Debug.Log("New lightning trap status: " + _isActive.Value);
    }

    private void OnEnable()
    {
        _isActive.OnValueChanged += OnActiveStatusChanged;
    }

    private void OnDisable()
    {
        _isActive.OnValueChanged -= OnActiveStatusChanged;
    }

    private void OnActiveStatusChanged(bool oldValue, bool newValue)
    {
        UpdateTrapEffects(newValue);
    }

    public string GetId()
    {
        return String.Empty;
    }
}