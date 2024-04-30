using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RobertInteractionController : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Grab")]
    [SerializeField] private Transform grabParent;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI promptText;

    private InteractableController _currentInteractable = null;
    private InteractableController _grabbedInteractable = null;

    private void Awake()
    {
        if (promptText)
        {
            promptText.text = string.Empty;
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.InputControls.Game.Interact.started += OnInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.InputControls.Game.Interact.started -= OnInteract;
    }

    private void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(ray.origin, 0.5f, ray.direction, out hit, interactionDistance, interactableLayer))
        {
            InteractableController interactable = hit.collider.GetComponent<InteractableController>();
            if (interactable)
            {
                if (_currentInteractable != interactable)
                {
                    ResetCurrentInteractable();
                }
                _currentInteractable = interactable;
                _currentInteractable.ToggleOutline(true);
                promptText.text = "Click / E to " + _currentInteractable.GetPromptMessage();
                return;
            }
        }

        ResetCurrentInteractable();
    }

    private void ResetCurrentInteractable()
    {
        if (_currentInteractable)
        {
            promptText.text = string.Empty;
            _currentInteractable.ToggleOutline(false);
            _currentInteractable = null;
        }
    }

    public void GrabInteractable(InteractableController grabInteractable)
    {
        if (_grabbedInteractable)
        {
            DropInteractable();
        }

        _grabbedInteractable = grabInteractable;
        _grabbedInteractable.transform.position = grabParent.transform.position;
        _grabbedInteractable.transform.SetParent(grabParent);
    }

    public void DropInteractable()
    {
        _grabbedInteractable.transform.parent = null;
        _grabbedInteractable = null;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_grabbedInteractable && _grabbedInteractable != _currentInteractable)
        {
            _grabbedInteractable.Interact();
        }

        if (!_currentInteractable)
        {
            return;
        }

        _currentInteractable.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactionDistance);
    }
}
