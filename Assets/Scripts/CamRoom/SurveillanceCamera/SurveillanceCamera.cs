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
        if (_controlled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                RotateUp();
            }
            if (Input.GetKey(KeyCode.S))
            {
                RotateDown();
            }
            if (Input.GetKey(KeyCode.A))
            {
                RotateLeft();
            }
            if (Input.GetKey(KeyCode.D))
            {
                RotateRight();
            }
            if (Input.GetKey(KeyCode.Q))
            {
                ZoomIn();
            }
            if (Input.GetKey(KeyCode.E))
            {
                ZoomOut();
            }
        }
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

    void RotateUp()
    {
        if (_controlled)
        {
            _currentAngleUpDown = Mathf.Min(_currentAngleUpDown + _rotationSpeed * Time.deltaTime, _maxAngleUpDown);
            camPivotUpDown.localRotation = Quaternion.Euler(_currentAngleUpDown, 0, 0);
        }
    }

    void RotateDown()
    {
        if (_controlled)
        {
            _currentAngleUpDown = Mathf.Max(_currentAngleUpDown - _rotationSpeed * Time.deltaTime, -_maxAngleUpDown);
            camPivotUpDown.localRotation = Quaternion.Euler(_currentAngleUpDown, 0, 0);
        }
    }

    void RotateLeft()
    {
        if (_controlled)
        {
            _currentAngleLeftRight = Mathf.Max(_currentAngleLeftRight - _rotationSpeed * Time.deltaTime, -_maxAngleLeftRight);
            camPivotLeftRight.localRotation = Quaternion.Euler(0, _currentAngleLeftRight, 0);
        }
    }

    void RotateRight()
    {
        if (_controlled)
        {
            _currentAngleLeftRight = Mathf.Min(_currentAngleLeftRight + _rotationSpeed * Time.deltaTime, _maxAngleLeftRight);
            camPivotLeftRight.localRotation = Quaternion.Euler(0, _currentAngleLeftRight, 0);
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
