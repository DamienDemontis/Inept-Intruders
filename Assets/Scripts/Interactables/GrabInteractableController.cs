using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInteractableController : InteractableController
{
    private Rigidbody _rb;

    private bool _isGrabbing = false;
    private Vector3 _startingPos = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _startingPos = transform.position;
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
            GameManager.Instance.RobertInteractionController.DropInteractable();
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
        } else
        {
            _rb.isKinematic = true;
            GameManager.Instance.RobertInteractionController.GrabInteractable(this);
        }

        _isGrabbing = !_isGrabbing;
    }
}
