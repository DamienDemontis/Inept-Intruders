using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    [SerializeField] private Vector3 activationTrigger = Vector3.one;
    [SerializeField] private LayerMask activationLayer;
    [Space]
    [SerializeField] private float activationTime = 0.5f;
    [SerializeField] private ToggleInteractableController linkedInteractable;

    [Header("Visuals")]
    [SerializeField] private Material inactiveMaterial;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private MeshRenderer targetMeshRenderer;

    private List<GameObject> _activationObjects = new List<GameObject>();

    private float _currentActivationTime = 0.0f;
    private bool _isActivated = true;

    private void Update()
    {
        DetectActivationObjects();

        bool lastActivated = _isActivated;

        if (_activationObjects.Count > 0 && !_isActivated)
        {
            _currentActivationTime += Time.deltaTime;

            if (_currentActivationTime >= activationTime)
            {
                _isActivated = true;
            }
        } else if (_activationObjects.Count == 0)
        {
            _isActivated = false;
            _currentActivationTime = 0.0f;
        }

        if (lastActivated != _isActivated)
        {
            if (_isActivated)
            {
                targetMeshRenderer.material = activeMaterial;
                linkedInteractable.Interact();
            } else
            {
                targetMeshRenderer.material = inactiveMaterial;
                linkedInteractable.CancelInteract();
            }
        }
    }

    private void DetectActivationObjects()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, activationTrigger / 2, Quaternion.identity, activationLayer);

        foreach (GameObject activationObject in _activationObjects)
        {
            bool foundObject = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject == activationObject)
                {
                    foundObject = true;
                    break;
                }
            }

            if (!foundObject)
            {
                _activationObjects.Remove(activationObject);
                break;
            }
        }

        if (_activationObjects.Count > 0)
        {
            return;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (!_activationObjects.Contains(hitColliders[i].gameObject))
            {
                _activationObjects.Add(hitColliders[i].gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, activationTrigger);
    }
}
