using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private Dictionary<string, IInteractable> interactableMap;

    void Start()
    {
        interactableMap = new Dictionary<string, IInteractable>();
    }

    void Update()
    {
        
    }

    public void SetInteractablesList(List<IInteractable> interactablesList)
    {
        if (interactablesList != null)
        {
            interactableMap.Clear();
        }

        foreach (IInteractable interactable in interactablesList)
        {
            interactableMap[interactable.GetId()] = interactable;
        }
    }

    public void AddInteractable(IInteractable interactable)
    {
        if (interactableMap == null)
        {
            interactableMap = new Dictionary<string, IInteractable>();
        }

        interactableMap[interactable.GetId()] = interactable;
    }

    public bool InteractWith(string interactableId)
    {
        if (!interactableMap.TryGetValue(interactableId, out IInteractable interactable))
        {
            Debug.LogWarning("[InteractableManager::InteractWith] " + interactableId + " : is not in the interactableMap.");
            return false;
        }

        interactable.Interact();

        return true;
    }

    /// <summary>
    /// Gets a list of all interactable IDs currently managed by this InteractableManager.
    /// </summary>
    public List<string> InteractableIdsList
    {
        get
        {
            if (interactableMap == null)
                return new List<string>();
            if (interactableMap.Keys.Count == 0)
                return new List<string>();
            else
                return new List<string>(interactableMap.Keys);
        }
    }

    public int Count
    {
        get { return interactableMap.Count;}
    }
}
