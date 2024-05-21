using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLockedDoor : ToggleInteractableController
{
    private DoorInteractableController _doorController;

    private void Start()
    {
        _doorController = GetComponent<DoorInteractableController>();
    }

    public override void Interact()
    {
        base.Interact();

        _doorController.IsLocked = false;
    }

    public override void CancelInteract()
    {
        base.CancelInteract();

        _doorController.IsLocked = true;
    }
}
