using System;
using UnityEngine;

public class FireThrower : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private BoxCollider fireCollider;
    private bool _isActive = false;

    private void Start()
    {
        Interact();   
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
        _isActive = !_isActive;
        if (_isActive)
        {
            fireParticles.Play();
        }
        else
        {
            fireParticles.Stop();
        }
        fireCollider.enabled = _isActive;
        Debug.Log("New fire thrower status:" + _isActive.ToString());
    }

    public string GetId()
    {
        return String.Empty;
    }
}
