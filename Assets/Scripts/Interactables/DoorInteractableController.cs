using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractableController : InteractableController
{
    private bool _isOpen = false;

    public override string GetPromptMessage()
    {
        if (!_isOpen)
        {
            return "Open";
        }

        return "Close";
    }

    public override void Interact()
    {
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
