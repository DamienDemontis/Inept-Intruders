using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField, Tooltip("Damage object when trigger collider entered, or simply when collision is registered")] private bool useTrigger;
    [SerializeField] private LayerMask damageLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (useTrigger || !damageLayer.Contains(collision.gameObject.layer))
        {
            return;
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        damageable.OnDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger || !damageLayer.Contains(other.gameObject.layer))
        {
            return;
        }

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable == null)
        {
            return;
        }

        damageable.OnDamage();
    }
}
