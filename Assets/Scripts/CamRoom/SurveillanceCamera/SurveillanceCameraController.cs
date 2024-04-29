using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SurveillanceCameraController : MonoBehaviour
{
    [SerializeField] private SurveillanceCamera surveillanceCamera;
    public SurveillanceCameraInputActions surveillanceCameraInputActions;

    private InputAction _look;
    private InputAction _zoom;

    void Start()
    {
        surveillanceCameraInputActions = new SurveillanceCameraInputActions();
    }

    private void OnEnable()
    {
        _look = surveillanceCameraInputActions.SurveillanceCamera.Look;
        _zoom = surveillanceCameraInputActions.SurveillanceCamera.ZoomInOut;

        _look.Enable();
        _zoom.Enable();

        _look.performed += Look;
        _zoom.performed += Zoom;
    }

    private void OnDisable()
    {
        _look.Disable();
        _zoom.Disable();
    }
    private void Look(InputAction.CallbackContext context)
    {
        Debug.Log("[CamGuyController::Look] You are looking good.");

        Quaternion cameraRotation = surveillanceCamera.camPivotLeftRight.rotation;

        Vector2 look = context.ReadValue<Vector2>();
        if (_look != null)
        {
            Quaternion newRotation = surveillanceCamera.camPivotLeftRight.rotation;
            newRotation.x += look.x;

            surveillanceCamera.camPivotLeftRight.rotation = newRotation;
        }
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        Debug.Log("[CamGuyController::Look] You are Zooming :).");


    }
}
