using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private DestroyPropagator objectPrefab;

    private void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        DestroyPropagator spawnedObject = Instantiate(objectPrefab, transform);
        spawnedObject.OnObjectDestroyed += OnObjectDestroyed;
    }

    public void OnObjectDestroyed(GameObject destroyedObject)
    {
        SpawnObject();
    }
}
