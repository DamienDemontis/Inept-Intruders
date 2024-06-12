using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : NetworkBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;

    private InputControls _inputControls;
    public InputControls InputControls => _inputControls;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this);
            return;
        }

        _instance = this;

        if (_inputControls == null)
        {
            _inputControls = new InputControls();
        }
    }

    private void OnEnable()
    {
        _inputControls.Game.Enable();
    }

    private void OnDisable()
    {
        _inputControls.Game.Disable();
    }

    public Vector2 GetMouseDelta()
    {
        return _inputControls.Game.Look.ReadValue<Vector2>();
    }
}
