using System;
using Unity.Netcode;
using UnityEngine;

public class FireThrower : NetworkBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private BoxCollider fireCollider;
    private NetworkVariable<bool> _isActive = new NetworkVariable<bool>();

    private void Start()
    {
        if (IsServer)
        {
            UpdateFireThrowerStatus(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (IsServer)
        {
            UpdateFireThrowerStatus(!_isActive.Value);
        }
        else
        {
            RequestFireThrowerStatusChangeServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestFireThrowerStatusChangeServerRpc(ServerRpcParams rpcParams = default)
    {
        UpdateFireThrowerStatus(!_isActive.Value);
    }

    private void UpdateFireThrowerStatus(bool newStatus)
    {
        _isActive.Value = newStatus;
        UpdateFireEffects(newStatus);
    }

    private void UpdateFireEffects(bool isActive)
    {
        fireCollider.enabled = isActive;
        if (isActive)
        {
            fireParticles.Play();
        }
        else
        {
            fireParticles.Stop();
        }
        Debug.Log("New fire thrower status: " + _isActive.Value);
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
        UpdateFireEffects(newValue);
    }

    public string GetId()
    {
        return String.Empty;
    }
}