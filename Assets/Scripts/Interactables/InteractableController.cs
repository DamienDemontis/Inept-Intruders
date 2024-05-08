using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptMessage;

    protected Animator _animator;
    private Outline _outline;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _outline = GetComponentInChildren<Outline>();
        ToggleOutline(false);
    }

    public void ToggleOutline(bool enabled)
    {
        if (!_outline)
        {
            return;
        }

        _outline.enabled = enabled;
    }

    public virtual string GetPromptMessage()
    {
        return promptMessage;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + name);
    }

    public string GetId()
    {
        return string.Empty;
    }
}
