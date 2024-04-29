using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour, IInteractable
{
    [SerializeField] private string _id;

    [Header("Camera")]
    [SerializeField] private RenderTexture _screenRenderTexture;
    [SerializeField] private CamGuyCamera  _camGuyCamera;
    [SerializeField] private Material      _screenMaterial;
    [SerializeField] private int           _currentCameraIndex = 0;

    private Material currentRenderTextureMaterial;

    public List<SurveillanceCamera> _roomSurveillanceCamerasList = new List<SurveillanceCamera>();
    private SurveillanceCamera _currentCamera;
    private int _lastCurrentCameraIndex;

    void Start()
    {
        _screenMaterial.mainTexture = _screenRenderTexture;
        _lastCurrentCameraIndex     = _currentCameraIndex;
    }

    void Update()
    {
        if (_lastCurrentCameraIndex != _currentCameraIndex)
        {
            Debug.Log($"[Monitor::Update] change camera index from {_lastCurrentCameraIndex} to {_currentCameraIndex}.");

            _lastCurrentCameraIndex = _currentCameraIndex;

            SetCamera(_currentCameraIndex);
        }
    }

    public void SetCamerasList(List<SurveillanceCamera> surveillanceCamerasList)
    {
        _roomSurveillanceCamerasList = surveillanceCamerasList;

        if (_roomSurveillanceCamerasList == null)
        {
            Debug.LogError($"[Monitor::SetCameras] The list is null.");
            return;
        }
        if (_roomSurveillanceCamerasList.Count == 0)
        {
            Debug.LogWarning($"[Monitor::SetCameras] No Surveillance Cameras");
            return;
        }

        _currentCameraIndex = 0;

        SetCamera(_currentCameraIndex);
    }


    private void SetCamera(int cameraIndex)
    {
        if (_roomSurveillanceCamerasList == null)
        {
            Debug.LogError($"[Monitor::SetCameras] The list is null.");
            return;
        }

        if (_roomSurveillanceCamerasList.Count == 0)
        {
            Debug.LogWarning($"[Monitor::SetCameras] No Surveillance Cameras");
            return;
        }

        _currentCamera.Controlled = false;
        _currentCamera.Deactivate();

        _currentCameraIndex     = cameraIndex;
        _lastCurrentCameraIndex = _currentCameraIndex;

        _currentCamera = _roomSurveillanceCamerasList[_currentCameraIndex % _roomSurveillanceCamerasList.Count];
        _currentCamera.Activate();
        _currentCamera.Controlled = true;

        _screenRenderTexture        = _currentCamera.CamRenderTexture;
        _screenMaterial.mainTexture = _screenRenderTexture;

        Debug.Log($"[Monitor::SetCameras] New camera : {_currentCamera.name}");
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"[Monitor::OnMouseUpAsButton] Monitor {_id} clicked.");

        Interact();
    }


    public void Interact()
    {
        if (_camGuyCamera == null)
        {
            Debug.LogError($"[Monitor::Interact] No Cam Guy Camera on Monitor {_id}, cannot position in front of monitor");
            return;
        }

        _camGuyCamera.StartTransition(this.transform, 0.9f, 1f);
    }

    public void SwitchToNextCamera()
    {
        if (_roomSurveillanceCamerasList == null)
        {
            Debug.LogError($"[Monitor::SwitchToNextCamera] No camera to look at on Monitor {_id}.");
            return;
        }

        _currentCamera = _roomSurveillanceCamerasList[(++_currentCameraIndex) % _roomSurveillanceCamerasList.Count];
        _screenMaterial.mainTexture = _currentCamera.CamRenderTexture;
    }

    public string GetId()
    {
        return _id;
    }

    public Texture ScreenMaterialTexture { set { _screenMaterial.mainTexture = value; } }
}
