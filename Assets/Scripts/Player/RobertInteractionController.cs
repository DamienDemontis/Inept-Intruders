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

    [Header("References")]
    [SerializeField] private TextMeshProUGUI promptText;

    InteractableController _currentInteractable;

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
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, interactableLayer)) {
            InteractableController interactable = hit.collider.GetComponent<InteractableController>();
            if (interactable)
            {
                if (_currentInteractable != interactable)
                {
                    ResetCurrentInteractable();
                }
                _currentInteractable = interactable;
                _currentInteractable.ToggleOutline(true);
                promptText.text = "Click to " + _currentInteractable.GetPromptMessage();
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

    private void OnInteract(InputAction.CallbackContext ctx)
    {
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
