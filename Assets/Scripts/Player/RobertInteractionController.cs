using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RobertInteractionController : MonoBehaviour, IDamageable
{
    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 0.25f;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Grab")]
    [SerializeField] private Transform grabParent;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI promptText;

    private InteractableController _currentInteractable = null;
    private InteractableController _grabbedInteractable = null;

    private RobertMovementController _movementController = null;

    public bool IsGrabbing => _grabbedInteractable != null;

    private void Awake()
    {
        if (promptText)
        {
            promptText.text = string.Empty;
        }

        _movementController = GetComponent<RobertMovementController>();
    }

    private void OnEnable()
    {
        InputManager inst = InputManager.Instance;

        if (inst == null)
        {
            Debug.LogWarning("InputManager is null");
            return;
        }

        InputManager.Instance.InputControls.Game.Interact.started += OnInteract;
    }

    private void OnDisable()
    {
        InputManager inst = InputManager.Instance;

        if (inst == null)
        {
            Debug.LogWarning("InputManager is null");
            return;
        }

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

        if (Physics.SphereCast(ray.origin, interactionRadius, ray.direction, out hit, interactionDistance, interactableLayer))
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
                promptText.text = _currentInteractable.GetPromptMessage();
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

    public bool GrabInteractable(InteractableController grabInteractable)
    {
        if (_grabbedInteractable)
        {
            DropInteractable();
            return false;
        }

        _grabbedInteractable = grabInteractable;
        _grabbedInteractable.transform.position = grabParent.transform.position;
        _grabbedInteractable.transform.SetParent(grabParent);
        return true;
    }

    public void DropInteractable()
    {
        StartCoroutine(DropInteractableSequence());
    }

    private IEnumerator DropInteractableSequence()
    {
        InteractableController tmpGrabInteractable = _grabbedInteractable;

        _movementController.ToggleMovement(false);

        _movementController.Animator.SetTrigger("DropObject");

        yield return new WaitForSeconds(0.9f);

        tmpGrabInteractable.transform.SetParent(null, true);
        tmpGrabInteractable.GetComponent<Rigidbody>().isKinematic = false;
        tmpGrabInteractable.GetComponent<Collider>().enabled = true;

        if (_grabbedInteractable == tmpGrabInteractable)
        {
            _grabbedInteractable = null;
        }

        _movementController.ToggleMovement(true);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!_currentInteractable)
        {
            if (_grabbedInteractable && _grabbedInteractable != _currentInteractable)
            {
                _grabbedInteractable.Interact();
            }
            return;
        }

        _currentInteractable.Interact();
    }

    public void OnDamage()
    {
        GameManager.Instance.TriggerRobertPlayerDeath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * interactionDistance);
    }
}
