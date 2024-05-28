using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointIndex = 0;
    [Space]
    [SerializeField] private float triggerRadius = 2.0f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField, Tooltip("Transform at which the player will spawn if he dies")] private Transform saveTransform;

    public Transform SaveTransform => saveTransform;

    private void Update()
    {
        if (GameManager.Instance.LastCheckpoint == checkpointIndex)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius, playerLayer);
        
        if (hitColliders.Length > 0)
        {
            GameManager.Instance.SaveLastCheckpoint(checkpointIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
