using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamGuyController : MonoBehaviour
{
    [SerializeField] public CamGuyInputActions camGuyInputActions;

    // Player
    private InputAction _interact;
    private InputAction _move;
    private InputAction _look;

    // Debug
    private InputAction _switchRoom;

    private CamRoom _camRoom;

    private void Awake()
    {
        camGuyInputActions = new CamGuyInputActions();
    }

    private void OnEnable()
    {
        _interact = camGuyInputActions.Player.Interact;
        _move     = camGuyInputActions.Player.Move;
        _look     = camGuyInputActions.Player.Look;

        _switchRoom = camGuyInputActions.Debug.SwitchRoom;

        _interact.Enable();
        _move.Enable();
        _look.Enable();

        _switchRoom.Enable();

        _interact.performed += Interact;
        _move.performed     += Move;
        _look.performed     += Look;

        _switchRoom.performed += SwitchToNextRoom;
    }

    private void OnDisable()
    {
        _interact.Disable();
        _move.Disable();
        _look.Disable();
    }
    private void Look(InputAction.CallbackContext context)
    {
    }

    private void Move(InputAction.CallbackContext context)
    {
    }

    private void Interact(InputAction.CallbackContext context)
    {
    }

    private void SwitchToNextRoom(InputAction.CallbackContext context)
    {
        if (_camRoom == null)
        {
            Debug.LogWarning("[CamGuyController::SwitchToNextRoom] boardButton null.");
            return;
        }

        _camRoom.SwitchToNextRoom();
    }
}
