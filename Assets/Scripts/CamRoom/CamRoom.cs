using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoom : MonoBehaviour
{
    [SerializeField] private Monitor _cameraMonitor;
    [SerializeField] private Board   _board;

    [Header("Room Config")]
    [SerializeField] private List<Room> _roomsList;
    [SerializeField] private int        _currentRoomIndex = 0;

    private int _lastCurrentRoomIndex;

    void Start()
    {
        if (_cameraMonitor == null)
        {
            Debug.LogError("[CamRoom::Start] Camera monitor is null.");
            return;
        }

        if (_roomsList == null || _roomsList.Count == 0)
        {
            Debug.LogError("[CamRoom::Start] Rooms list is null or empty.");
            return;
        }

        if (_board == null)
        {
            Debug.LogError("[CamRoom::Start] Button board is null.");
            return;
        }

        //_board.RoomsList = _roomsList;
        _board.Room = _roomsList[_currentRoomIndex];

        if (_roomsList.Count <= _currentRoomIndex)
        {
            Debug.LogWarning("[CamRoom::Start] no rooms attributed to the CamRoom.");
        }
        else
        {
            _cameraMonitor.SetCamerasList(_roomsList[_currentRoomIndex].SurveillanceCamerasList);
        }
    }

    void Update()
    {
        if (_lastCurrentRoomIndex != _currentRoomIndex)
        {
            Debug.Log($"[Monitor::Update] change camera index from {_lastCurrentRoomIndex} to {_currentRoomIndex}.");

            _lastCurrentRoomIndex = _currentRoomIndex;

            if (_currentRoomIndex >= _roomsList.Count)
            {
                _currentRoomIndex = 0;
                Debug.Log($"[Monitor::Update] Room index to high, no more than {_roomsList.Count} are availables.");
            }

            _cameraMonitor.SetCamerasList(_roomsList[_currentRoomIndex].SurveillanceCamerasList);
            _board.Room = _roomsList[_currentRoomIndex];
        }
    }

    public void SwitchToNextRoom()
    {
        if (_roomsList == null || _roomsList.Count == 0)
        {
            Debug.LogWarning("[Board::SwitchToNextRoom] No rooms available to switch.");
            return;
        }

        if (_roomsList.Count > _currentRoomIndex + 1)
        {
            _currentRoomIndex++;
        }

        _board.Room = _roomsList[_currentRoomIndex];

        Debug.Log($"[Board::SwitchToNextRoom] Switched to room: {_roomsList[_currentRoomIndex].name}");
    }

    public void SwitchToPreviousRoom()
    {
        if (_roomsList == null || _roomsList.Count == 0)
        {
            Debug.LogWarning("[Board::SwitchToPreviousRoom] No rooms available to switch.");
            return;
        }

        if (_currentRoomIndex - 1 >= 0)
        {
            _currentRoomIndex--;
        }

        _board.Room = _roomsList[_currentRoomIndex];

        Debug.Log($"[Board::SwitchToNextRoom] Switched to room: {_roomsList[_currentRoomIndex].name}");
    }

    public List<Room> RoomsList
    {
        get { return _roomsList; }
        set { _roomsList = value; }
    }
}
