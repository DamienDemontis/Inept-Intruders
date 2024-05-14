using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField, Tooltip("Trigger player death when trigger collider entered, or simply when collision is registered")] private bool useTrigger;
    [SerializeField] private LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger || !playerLayer.Contains(collision.gameObject.layer))
        {
            return;
        }

        GameManager.Instance.TriggerRobertPlayerDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger || !playerLayer.Contains(other.gameObject.layer))
        {
            return;
        }

        GameManager.Instance.TriggerRobertPlayerDeath();
    }
}
