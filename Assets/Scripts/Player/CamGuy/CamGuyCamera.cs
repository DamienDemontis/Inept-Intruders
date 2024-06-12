using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGuyCamera : MonoBehaviour
{
    [SerializeField] private CamGuy _camGuy;

    [Header("Transition Config")]
    [SerializeField] private float          _transitionDuration = 2.5f;
    [SerializeField] private AnimationCurve _transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Camera Rotation")]
    [SerializeField] private Vector2 sensitivity = Vector2.zero;
    [SerializeField] private float   maxRotationSpeed = 40f;
    [SerializeField] private bool    clampXRotation = true;
    [SerializeField] private bool    clampYRotation = false;
    [SerializeField] private Vector2 minRotation = Vector2.zero;
    [SerializeField] private Vector2 maxRotation = Vector2.zero;

    [Header("References")]
    [SerializeField] private Transform orientation;

    private Vector2 _cameraRotation = Vector2.zero;

    private Transform _target;
    private float     _distanceFromTarget;
    private Transform _baseTransfomer;

    private void Start()
    {
        _baseTransfomer = _camGuy.transform;
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
        _transitionDuration = transitionDuration;
        _distanceFromTarget = distanceFromTarget;

        StartCoroutine(Transition());
    }

    public void ResetToBase(float transitionDuration = 2.5f)
    {
        StartTransition(_baseTransfomer, 0.0f, transitionDuration);
    }

    IEnumerator Transition()
    {
        float t = 0.0f;
        Vector3 startingPos    = transform.position;
        Quaternion startingRot = transform.rotation;

        Vector3 directionToTarget = _target.position - transform.position;
        directionToTarget.Normalize();
        Vector3 modifiedTargetPos = _target.position - directionToTarget * _distanceFromTarget;

        Quaternion targetRotation = _target.rotation;

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
