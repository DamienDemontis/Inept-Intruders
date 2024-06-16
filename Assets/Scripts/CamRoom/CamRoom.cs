using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoom : MonoBehaviour
{
    [SerializeField] private Monitor _cameraMonitor;
    [SerializeField] private Board   _board;

    [Header("Room Config")]
    [SerializeField] private List<Room> _roomsList = new List<Room>();
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
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                if (room != null)
                {
                    Debug.Log($"[CamRoom::Start] Added room in cam room.");
                    _roomsList.Add(room);
                }
            }

            if (_roomsList.Count == 0)
            {
                Debug.LogWarning("[CamRoom::Start] No rooms found in the CamRoom.");
                return;
            }
        }

        if (_board == null)
        {
            Debug.LogError("[CamRoom::Start] Button board is null.");
            return;
        }

        _board.Room = _roomsList[_currentRoomIndex];

        if (_roomsList.Count <= _currentRoomIndex)
        {
            Debug.LogWarning("[CamRoom::Start] no rooms attributed to the CamRoom.");
        }
        else
        {
            Debug.Log($"[CamRoom::Start] Adding list of camera from room to monitor");
            _cameraMonitor.SetCamerasList(_roomsList[_currentRoomIndex].SurveillanceCamerasList);
        }
    }

    void Update()
    {
        if (_roomsList == null || _roomsList.Count == 0)
        {
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                if (room != null)
                {
                    Debug.Log($"[CamRoom::Start] Added room in cam room.");
                    _roomsList.Add(room);
                }
            }

            if (_roomsList.Count == 0)
            {
                Debug.LogWarning("[CamRoom::Start] No rooms found in the CamRoom.");
                return;
            }
        }

        if (_lastCurrentRoomIndex != _currentRoomIndex)
        {
            Debug.Log($"[CamRoom::Update] change camera index from {_lastCurrentRoomIndex} to {_currentRoomIndex}.");

            _lastCurrentRoomIndex = _currentRoomIndex;

            if (_currentRoomIndex >= _roomsList.Count)
            {
                _currentRoomIndex = 0;
                Debug.Log($"[CamRoom::Update] Room index to high, no more than {_roomsList.Count} are availables.");
            }

            _cameraMonitor.SetCamerasList(_roomsList[_currentRoomIndex].SurveillanceCamerasList);
            _board.Room = _roomsList[_currentRoomIndex];
        }
    }

    public void SwitchToNextRoom()
    {
        if (_roomsList == null || _roomsList.Count == 0)
        {
            Debug.LogWarning("[CamRoom::SwitchToNextRoom] No rooms available to switch.");
            return;
        }

        if (_roomsList.Count > _currentRoomIndex + 1)
        {
            _currentRoomIndex++;
        }

        _board.Room = _roomsList[_currentRoomIndex];

        Debug.Log($"[CamRoom::SwitchToNextRoom] Switched to room: {_roomsList[_currentRoomIndex].name}");
    }

    public void SwitchToPreviousRoom()
    {
        if (_roomsList == null || _roomsList.Count == 0)
        {
            Debug.LogWarning("[CamRoom::SwitchToPreviousRoom] No rooms available to switch.");
            return;
        }

        if (_currentRoomIndex - 1 >= 0)
        {
            _currentRoomIndex--;
        }

        _board.Room = _roomsList[_currentRoomIndex];

        Debug.Log($"[CamRoom::SwitchToNextRoom] Switched to room: {_roomsList[_currentRoomIndex].name}");
    }

    public List<Room> RoomsList
    {
        get { return _roomsList; }
        set { _roomsList = value; }
    }
}
