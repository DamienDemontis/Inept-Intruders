using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private string   _id;
    [SerializeField] public Animator animator;

    [SerializeReference] public List<IInteractable> interactablesList;

    private const string _defaultButtonId = "NoIdSpecified";

    void Start()
    {
        _id = _id == "" ? _defaultButtonId : _id;

        if (animator == null)
        {
            Debug.LogWarning($"[Button::Start] animator is null on button {_id}");
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"[Button::OnMouseUpAsButton] Button {_id} clicked.");

        Interact();
    }

    public void Interact()
    {
        if (interactablesList == null)
        {
            Debug.LogWarning($"[Button::OnMouseUpAsButton] No interactables given to button {_id}.");
        }

        foreach (IInteractable interactable in interactablesList)
        {
            if (interactable == null)
            {
                Debug.LogWarning($"[Button::OnMouseUpAsButton] interactable null on button {_id}.");
                continue;
            }

            interactable.Interact();
        }
    }

    public string GetId()
    {
        return _id;
    }

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }
}
