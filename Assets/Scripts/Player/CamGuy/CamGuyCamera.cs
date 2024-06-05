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
    private Vector3   _positionFromCamGuy;

    private Vector3    _baseLocalPosition;
    private Vector3    _basePosition;
    private Quaternion _baseRotation;

    private void Start()
    {
        _baseLocalPosition = transform.localPosition;
        _basePosition = _camGuy.transform.position;
        _baseRotation = _camGuy.transform.rotation;
    }

    private void Update()
    {
        if (_camGuy == null)
        {
            Debug.LogWarning("[CamGuyCamera::Update] cam guy null");
            return;
        }

        //_basePosition = _camGuy.transform.position + _baseLocalPosition;
        //_baseRotation = _camGuy.transform.rotation;

        //UpdateCameraRotation();
    }

    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void UpdateCameraRotation()
    {
        Vector2 mouseDelta_ = InputManager.Instance.GetMouseDelta();
        Vector2 mouseDelta = mouseDelta_ * sensitivity * Time.deltaTime;
        _cameraRotation += new Vector2(-mouseDelta.y, mouseDelta.x);

        if (clampXRotation)
        {
            _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, minRotation.x, maxRotation.x);
        }
        if (clampYRotation)
        {
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y, minRotation.y, maxRotation.y);
        }

        Quaternion targetRotation = Quaternion.Euler(_cameraRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        orientation.rotation = Quaternion.Euler(0, _cameraRotation.y, 0);
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
        //Quaternion startingRot = transform.rotation;

        Vector3 directionToTarget = _target.position - transform.position;
        directionToTarget.Normalize();
        Vector3 modifiedTargetPos = _target.position - directionToTarget * _distanceFromTarget;

        //Quaternion targetRotation = _target.rotation;

        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / _transitionDuration);

            float curveT = _transitionCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startingPos, modifiedTargetPos, curveT);
            //transform.rotation = Quaternion.Slerp(startingRot, targetRotation, curveT);

            yield return null;
        }

        transform.position = modifiedTargetPos;
        //transform.rotation = targetRotation;
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
