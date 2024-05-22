using UnityEngine;

public class FireThrower : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private BoxCollider fireCollider;
    [SerializeField] private bool isActive = true;

    private void Start()
    {
        SetFireThrowerStatus(isActive);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetFireThrowerStatus(!isActive);   
        }
    }

    private void SetFireThrowerStatus(bool status)
    {
        if (status)
        {
            fireParticles.Play();
        }
        else
        {
            fireParticles.Stop();
        }
        fireCollider.enabled = status;
        isActive = status;
        Debug.Log("New fire thrower status:" + isActive.ToString());
    }
}
