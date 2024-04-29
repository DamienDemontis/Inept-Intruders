using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] public string   id;
    [SerializeField] public Animator animator;

    [SerializeReference] public List<IInteractable> interactablesList;

    private const string _defaultButtonId = "NoIdSpecified";

    void Start()
    {
        id = id == "" ? _defaultButtonId : id;

        if (animator == null)
        {
            Debug.LogWarning($"[Button::Start] animator is null on button {id}");
        }
    }
    private void OnMouseUpAsButton()
    {
        Debug.Log($"[Button::OnMouseUpAsButton] Button {id} clicked.");

        Interact();
    }

    public void Interact()
    {
        if (interactablesList == null)
        {
            Debug.LogWarning($"[Button::OnMouseUpAsButton] No interactables given to button {id}.");
        }

        foreach (IInteractable interactable in interactablesList)
        {
            if (interactable == null)
            {
                Debug.LogWarning($"[Button::OnMouseUpAsButton] interactable null on button {id}.");
                continue;
            }

            interactable.Interact();
        }
    }

    public string GetId()
    {
        return id;
    }
}
