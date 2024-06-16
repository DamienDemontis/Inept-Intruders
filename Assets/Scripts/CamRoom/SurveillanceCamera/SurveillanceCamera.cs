using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
{
    [SerializeField] private string _id;

    [Header("Camera Config")]
    [SerializeField] private Camera     _camera;
    [SerializeField] private Vector2Int _resolution = new Vector2Int(1920, 1080);
    [SerializeField] private int        _framerate = 24;
    [SerializeField] private Camera     playerCamera;

    [Header("Movement Settings")]
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _zoomSpeed = 1.1f;
    [SerializeField] private float _minFOV = 15f;
    [SerializeField] private float _maxFOV = 120f;

    [SerializeField] public Transform camPivotLeftRight;
    [SerializeField] public Transform camPivotUpDown;

    [Header("Rotation Limits")]
    [SerializeField] private float _maxAngleLeftRight = 60f;
    [SerializeField] private float _maxAngleUpDown = 45f;

    [Header("")]
    [SerializeField] private bool _controlled = false;

    private float _currentAngleLeftRight = 0f;
    private float _currentAngleUpDown    = 0f;

    private RenderTexture _screenFeedRenderTexture;

    void Start()
    {
        _screenFeedRenderTexture = new RenderTexture(_resolution.x, _resolution.y, _framerate);
        _camera.targetTexture = _screenFeedRenderTexture;
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        TargetToPlayerCameraWorld(new Vector2(mousePos.x, mousePos.y));
    }

    public void TargetToPlayerCameraWorld(Vector2 look)
    {
        if (playerCamera == null)
        {
            Debug.LogWarning($"[SurveillanceCamera::TargetToPlayerCameraWorld] No player camera set: {_id}");
            return;
        }

        Vector3 target = playerCamera.ScreenToWorldPoint(new Vector3(look.x, look.y, playerCamera.nearClipPlane));
        camPivotLeftRight.transform.LookAt(target, Vector3.up);
    }

    public void ZoomIn()
    {
        if (_controlled)
        {
            _camera.fieldOfView = Mathf.Max(_camera.fieldOfView - _zoomSpeed * Time.deltaTime, _minFOV);
        }
    }

    public void ZoomOut()
    {
        if (_controlled)
        {
            _camera.fieldOfView = Mathf.Min(_camera.fieldOfView + _zoomSpeed * Time.deltaTime, _maxFOV);
        }
    }

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    public RenderTexture CamRenderTexture { get { return _screenFeedRenderTexture; } }

    public bool Controlled { get { return _controlled; } set { _controlled = value; } }

    public Camera CamGuyCamera { get { return playerCamera; } set { value = playerCamera; } }
}
