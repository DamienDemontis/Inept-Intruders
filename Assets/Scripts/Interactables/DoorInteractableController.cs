using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractableController : InteractableController
{
    [SerializeField] private bool _isLocked = false;

    private bool _isOpen = false;

    public bool IsLocked {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

    public override string GetPromptMessage()
    {
        if (_isLocked && !_isOpen)
        {
            return "Locked";
        }

        if (!_isOpen)
        {
            return "Open";
        }

        return "Close";
    }

    public override void Interact()
    {
        if (_isLocked && !_isOpen)
        {
            return;
        }

        base.Interact();

        if (!_isOpen)
        {
            _animator.SetTrigger("Open");
        } else
        {
            _animator.SetTrigger("Close");
        }

        _isOpen = !_isOpen;
    }
}
