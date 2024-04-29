using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] public string id = string.Empty;

    public string GetId()
    {
        return id;
    }

    public void Interact()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning($"[InteractableTest::Interact] Rigid body of interacrable test {id} is null.");
            return;
        }

        rb.useGravity = true;
    }
}
