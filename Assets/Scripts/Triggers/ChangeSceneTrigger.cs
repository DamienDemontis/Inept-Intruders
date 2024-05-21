using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [Space]
    [SerializeField] private float triggerRadius = 2.0f;
    [SerializeField] private LayerMask playerLayer;

    private bool _wasTriggered = false;

    private void Update()
    {
        if (_wasTriggered)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerRadius, playerLayer);
        
        if (hitColliders.Length > 0)
        {
            _wasTriggered = true;
            StartCoroutine(GameManager.Instance.ChangeScene(nextSceneName));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
