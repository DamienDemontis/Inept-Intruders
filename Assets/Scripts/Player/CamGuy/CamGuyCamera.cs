using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CamGuyCamera : MonoBehaviour
{
    [SerializeField] private CamGuy _camGuy;

    [Header("Transition Config")]
    [SerializeField] private float          _transitionDuration = 2.5f;
    [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Transform _target;
    private float     _distanceFromTarget;
    private Vector3   _positionFromCamGuy;

    private Vector3    _baseLocalPosition;
    private Vector3    _basePosition;
    private Quaternion _baseRotation;

    private void Start()
    {
        _baseLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_camGuy == null)
        {
            Debug.LogWarning("[CamGuyCamera::Update] cam guy null");
            return;
        }

        _basePosition = _camGuy.transform.position + _baseLocalPosition;
        _baseRotation = _camGuy.transform.rotation;
    }

    public void StartTransition(Transform target, float distanceFromTarget, float transitionDuration)
    {
        _target             = target;
        _transitionDuration = transitionDuration;
        _distanceFromTarget = distanceFromTarget;

        StartCoroutine(Transition());
    }

    public void ResetToBase(float transitionDuration = 2.5f)
    {
        StartCoroutine(ResetTransition());
    }

    IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 startingPos    = transform.position;
        Quaternion startingRot = transform.rotation;

        Vector3 directionToTarget = _target.position - transform.position;
        directionToTarget.Normalize();
        Vector3 modifiedTargetPos = _target.position - directionToTarget * _distanceFromTarget;

        Quaternion targetRotation = Quaternion.Inverse(_target.rotation);

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / _transitionDuration);

            float curveT = _transitionCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startingPos, modifiedTargetPos, curveT);
            transform.rotation = Quaternion.Slerp(startingRot, targetRotation, curveT);

            yield return null;
        }

        transform.position = modifiedTargetPos;
        transform.rotation = targetRotation;
    }

    IEnumerator ResetTransition()
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        Vector3 directionToTarget = _basePosition - transform.position;
        directionToTarget.Normalize();
        Vector3 modifiedTargetPos = _basePosition - directionToTarget;

        Quaternion targetRotation = _baseRotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / _transitionDuration);

            float curveT = _transitionCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startingPos, modifiedTargetPos, curveT);
            transform.rotation = Quaternion.Slerp(startingRot, targetRotation, curveT);

            yield return null;
        }

        transform.position = modifiedTargetPos;
        transform.rotation = targetRotation;
    }

}
