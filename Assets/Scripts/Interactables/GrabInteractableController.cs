using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInteractableController : InteractableController
{
    private Rigidbody _rb;
    private Collider _collider;

    private bool _isGrabbing = false;
    private Vector3 _startingPos = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _startingPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.y < -1.0f)
        {
            transform.position = _startingPos;
        }
    }

    public override string GetPromptMessage()
    {
        if (!_isGrabbing)
        {
            return "Grab";
        }

        return "Drop";
    }

    public override void Interact()
    {
        base.Interact();

        if (_isGrabbing)
        {
            GameManager.Instance.RobertReferences.RobertInteractionController.DropInteractable();
        } else
        {
            bool canGrab = GameManager.Instance.RobertReferences.RobertInteractionController.GrabInteractable(this);

            if (!canGrab)
            {
                return;
            }

            _rb.isKinematic = true;
            _collider.enabled = false;
        }

        _isGrabbing = !_isGrabbing;
    }
}
