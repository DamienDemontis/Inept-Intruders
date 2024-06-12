using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RobertCameraController : NetworkBehaviour
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
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener playerAudioSource;
    
    private Vector2 _cameraRotation = Vector2.zero;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Debug.Log("I deactivated an Audio Source");
            playerCamera.gameObject.SetActive(false);
            playerAudioSource.enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
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
        transform.rotation = targetRotation;
        orientation.rotation = Quaternion.Euler(0, _cameraRotation.y, 0);
    }

    public void ForceYRotation(float angleY)
    {
        _cameraRotation.y = angleY;
        orientation.rotation = Quaternion.Euler(0, angleY, 0);
    }
}
