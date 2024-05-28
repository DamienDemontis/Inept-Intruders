using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGuy : MonoBehaviour
{
    [SerializeField] public CamRoom camRoom;
    [SerializeField] private CamGuyController camGuyController;

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
