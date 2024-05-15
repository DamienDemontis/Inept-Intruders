using UnityEngine;

public class FireThrower : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private BoxCollider fireCollider;
    [SerializeField] private bool isActive = true;

    private void Start()
    {
        if (isActive)
        {
            fireCollider.enabled = true;
            fireParticles.Play();
        }
        else
        {
            fireCollider.enabled = false;
            fireParticles.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFire();   
        }
    }

    public void ToggleFire()
    {
        if (isActive)
        {
            fireCollider.enabled = false;
            fireParticles.Stop();
            isActive = false;
        }
        else
        {
            fireCollider.enabled = true;
            fireParticles.Play();
            isActive = true;
        }
        Debug.Log("Fire thrower toggled, current state:" + isActive.ToString());
    }
}
