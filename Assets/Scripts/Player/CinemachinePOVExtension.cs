using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngle = 80f;
    
    private Vector3 _startRotation;

    protected override void Awake()
    {
        base.Awake();

        _startRotation = transform.localRotation.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (!InputManager.Instance)
        {
            return;
        }

        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaAim = InputManager.Instance.GetMouseDelta();
                _startRotation.x += deltaAim.x * verticalSpeed * deltaTime;
                _startRotation.y += deltaAim.y * horizontalSpeed * deltaTime;
                _startRotation.y = Mathf.Clamp(_startRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-_startRotation.y, _startRotation.x, 0f);
            }
        }
    }
}
