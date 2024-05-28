using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractableVisibility : ToggleInteractableController
{
    [SerializeField] private GameObject targetObject;

    public override void Interact()
    {
        base.Interact();

        targetObject.SetActive(true);
    }

    public override void CancelInteract()
    {
        base.CancelInteract();

        targetObject.SetActive(false);
    }
}
