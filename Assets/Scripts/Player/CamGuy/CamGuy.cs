using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGuy : MonoBehaviour
{
    [SerializeField] public CamRoom camRoom;
    [SerializeField] private CamGuyController camGuyController;
    [SerializeField] private Vector3 positionFromCamRoom = new Vector3(0.8f, 1.27f, 2);


    private CamGuyState _camGuyState;

    public enum CamGuyState
    {
        FocusedOnCameraMonitor,
        FocusedOnMapMonitor,
        Unfocused,
    }

    void Start()
    {
        _camGuyState = CamGuyState.Unfocused;

        StartPosition();

        //    if (camRoom.board == null)
        //    {
        //        Debug.LogWarning("[CamGuy::Start] No button board on the Cam Guy.");
        //    }
        //    else
        //    {
        //        if (camGuyController == null)
        //        {
        //            Debug.LogWarning("[CamGuy::Start] No controller on the Cam Guy.");
        //        }
        //        else
        //        {
        //            camGuyController.ButtonBoard = camRoom.board;
        //        }
        //    }
    }

    void StartPosition()
    {
        if (camRoom == null)
        {
            Debug.LogError("[CamGuy::StartPosition] No Cam Room.");
            return;
        }

        transform.position = new Vector3(
            camRoom.transform.position.x - positionFromCamRoom.x,
            camRoom.transform.position.y + positionFromCamRoom.y,
            camRoom.transform.position.z - positionFromCamRoom.z
        );
    }

    void Update()
    {
        switch(_camGuyState)
        {
            case CamGuyState.FocusedOnCameraMonitor: break;
            case CamGuyState.FocusedOnMapMonitor:    break;
            case CamGuyState.Unfocused:              break;
            default:                                 break;
        }
    }
}
