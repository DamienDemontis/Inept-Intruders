using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CamGuyCamera : NetworkBehaviour
{
    [SerializeField] private CamGuy _camGuy;

    [Header("Transition Config")]
    [SerializeField] private float          transitionDuration = 2.5f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Transform      toFollow;

    private Transform _target;
    private float     _distanceFromTarget;
    private Transform _baseTransfomer;

    private bool _isTransitioning = false;

    private void Start()
    {
        _baseTransfomer = toFollow.transform;
    }

    private void Update()
    {
        if (_camGuy == null)
        {
            Debug.LogWarning("[CamGuyCamera::Update] cam guy null");
            return;
        }
    }

    public void StartTransition(Transform target, float distanceFromTarget, float transitionDuration)
    {
        _target             = target;
        this.transitionDuration = transitionDuration;
        _distanceFromTarget = distanceFromTarget;

        if (!_isTransitioning)
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        Debug.Log($"[CamGuyCamera::Transition] _baseTransfomer : {_baseTransfomer.position}");
        _isTransitioning = true;

        float t = 0.0f;
        Vector3 startingPos    = toFollow.position;
        Quaternion startingRot = toFollow.rotation;

        Vector3 directionToTarget = _target.position - toFollow.position;
        directionToTarget.Normalize();
        Vector3 modifiedTargetPos = _target.position - directionToTarget * _distanceFromTarget;

        Quaternion targetRotation = _target.rotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            float curveT = transitionCurve.Evaluate(t);

            toFollow.position = Vector3.Lerp(startingPos, modifiedTargetPos, curveT);
            toFollow.rotation = Quaternion.Slerp(startingRot, targetRotation, curveT);

            yield return null;
        }

        toFollow.position = modifiedTargetPos;
        toFollow.rotation = targetRotation;

        _isTransitioning = false;
    }
}
