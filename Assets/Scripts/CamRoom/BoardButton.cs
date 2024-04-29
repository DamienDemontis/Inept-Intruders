using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardButton : Button
{
    [Header("Button Status")]
    [SerializeField] private bool _activated = false;

    [Header("Button Cap Materials")]
    [SerializeField] private Material _capMatActivated;
    [SerializeField] private Material _capMatDeactidated;
    [SerializeField] private Renderer _capRenderer;

    void Start()
    {
        if (_capMatActivated == null || _capMatDeactidated == null)
        {
            Debug.LogWarning("[BoardButton::Start] No material for capMatActivated or capMatDeactidated.");
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"[BoardButton::OnMouseUpAsButton] Button {id} clicked");

        if (animator == null)
        {
            Debug.LogWarning($"[BoardButton::OnMouseUpAsButton] Animator is null for button {id}.");
        }
        else
        {
            animator.SetTrigger("OnClick");
        }

        if (!_activated)
        {
            Debug.Log($"[BoardButton::OnMouseUpAsButton] Button {id} disabled.");
            return;
        }

        Interact();
    }

    public bool Activated
    {
        get
        {
            return _activated;
        }
        set
        {
            _activated = value;

            if (_capRenderer != null)
            {
                _capRenderer.material = _activated ? _capMatActivated : _capMatDeactidated;
            }
        }
    }
}
