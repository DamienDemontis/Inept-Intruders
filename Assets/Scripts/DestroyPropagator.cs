using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPropagator : MonoBehaviour
{
    public event System.Action<GameObject> OnObjectDestroyed;

    private void OnDestroy()
    {
        if (OnObjectDestroyed == null)
        {
            return;
        }

        OnObjectDestroyed.Invoke(gameObject);
    }
}
