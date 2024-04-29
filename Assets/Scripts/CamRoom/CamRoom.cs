using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoom : MonoBehaviour
{
    [SerializeField] public Monitor cameraMonitor;
    [SerializeField] public Board   board;

    [Header("Room Config")]
    [SerializeField] public List<Room> roomsList;
    [SerializeField] private int       _currentRoomIndex = 0;

    private int _lastCurrentRoomIndex;

    void Start()
    {
        if (cameraMonitor == null)
        {
            Debug.LogError("[CamRoom::Start] Camera monitor is null.");
            return;
        }

        if (roomsList == null || roomsList.Count == 0)
        {
            Debug.LogError("[CamRoom::Start] Rooms list is null or empty.");
            return;
        }

        if (board == null)
        {
            Debug.LogError("[CamRoom::Start] Button board is null.");
            return;
        }

        board.roomsList = roomsList;

        if (roomsList.Count <= _currentRoomIndex)
        {
            Debug.LogWarning("[CamRoom::Start] no rooms attributed to the CamRoom.");
        }
        else
        {
            cameraMonitor.SetCamerasList(roomsList[_currentRoomIndex].SurveillanceCamerasList);
        }
    }

    void Update()
    {
        if (_lastCurrentRoomIndex != _currentRoomIndex)
        {
            Debug.Log($"[Monitor::Update] change camera index from {_lastCurrentRoomIndex} to {_currentRoomIndex}.");

            _lastCurrentRoomIndex = _currentRoomIndex;
            cameraMonitor.SetCamerasList(roomsList[_currentRoomIndex].SurveillanceCamerasList);
            //_currentCamera = roomsList[_currentRoomIndex % roomsList.Count];
        }
    }
}
