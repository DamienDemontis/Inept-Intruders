using UnityEngine;

public class LightningTrap : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftLightningParticles;
    [SerializeField] private ParticleSystem rightLightningParticles;
    [SerializeField] private BoxCollider lightningCollider;
    [SerializeField] private bool isActive = true;

    private void Start()
    {
        SetLightningTrapStatus(isActive);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetLightningTrapStatus(!isActive);   
        }
    }

    private void SetLightningTrapStatus(bool status)
    {
        if (status)
        {
            leftLightningParticles.Play();
            rightLightningParticles.Play();
        }
        else
        {
            leftLightningParticles.Stop();
            rightLightningParticles.Stop();
        }
        lightningCollider.enabled = status;
        isActive = status;
        Debug.Log("New lightning trap status:" + isActive.ToString());
    }
}
