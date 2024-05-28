using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorButton : MonoBehaviour, IInteractable
{
    enum ButtonType
    {
        Next,
        Previous,
        Tab,
        None
    }

    [SerializeField] private ButtonType _buttonType = ButtonType.None;
    [SerializeField] private Monitor _monitor;

    [SerializeField] private string _id;
    [SerializeField] private Animator _animator;

    private const string _defaultButtonId = "NoIdSpecified";

    void Start()
    {
        _id = _id == "" ? _defaultButtonId : _id;

        if (_animator == null)
        {
            Debug.LogWarning($"[Button::Start] animator is null on button {_id}");
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"[Button::OnMouseUpAsButton] Button {_id} clicked.");

        Interact();
    }

    public void Interact()
    {
        switch (_buttonType)
        {
            case ButtonType.Next:
                Debug.Log($"[MonitorButton::Interact] Next button clicked.");
                _monitor.SwitchToNextCamera();
                break;
            case ButtonType.Previous:
                Debug.Log($"[MonitorButton::Interact] Previous button clicked.");
                _monitor.SwitchToPreviousCamera();
                break;
            case ButtonType.Tab:
                Debug.Log($"[MonitorButton::Interact] Tab.");
                break;
            case ButtonType.None:
                Debug.LogWarning($"[MonitorButton::Interact] Button type not set.");
                break;
            default:
                Debug.LogWarning($"[MonitorButton::Interact] Button type not set.");
                break;
        }
    }

    public Monitor Monitor
    {
        get => _monitor;
        set => _monitor = value;
    }

    public string GetId()
    {
        return _id;
    }
}
