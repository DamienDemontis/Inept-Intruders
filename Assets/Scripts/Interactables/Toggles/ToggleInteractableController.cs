using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractableController : MonoBehaviour, IInteractable
{
    public virtual void Interact()
    {
        Debug.Log("Toggle interacted with " + name);
    }

    public virtual void CancelInteract()
    {
        Debug.Log("Toggle cancel interacted with " + name);
    }

    public string GetId()
    {
        return string.Empty;
    }
}
