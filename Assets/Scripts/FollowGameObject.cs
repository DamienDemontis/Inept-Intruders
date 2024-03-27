using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    private void LateUpdate()
    {
        transform.position = followTarget.position;
    }

    private void OnValidate()
    {
        transform.position = followTarget.position;
    }
}
