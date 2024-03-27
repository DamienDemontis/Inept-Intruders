using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertCameraController : MonoBehaviour
{
    [Header("Camera Rotation")]
    [SerializeField] private Vector2 sensitivity = Vector2.zero;
    [SerializeField] private float maxRotationSpeed = 40f;
    [SerializeField] private bool clampXRotation = true;
    [SerializeField] private bool clampYRotation = false;
    [SerializeField] private Vector2 minRotation = Vector2.zero;
    [SerializeField] private Vector2 maxRotation = Vector2.zero;

    [Header("References")]
    [SerializeField] private Transform orientation;
    
    private Vector2 _cameraRotation = Vector2.zero;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        Vector2 mouseDelta = InputManager.Instance.GetMouseDelta() * sensitivity * Time.deltaTime;
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
}