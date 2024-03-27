using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobertMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform modelTransform;

    private CharacterController _characterController;
    private Animator _animator;

    private bool _isGrounded = false;
    private float _gravityValue = -9.81f;
    private float _gravityPull = 0;

    private Vector3 _moveVector;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = modelTransform.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InputManager.Instance.InputControls.Game.Move.started += OnMove;
        InputManager.Instance.InputControls.Game.Move.performed += OnMove;
        InputManager.Instance.InputControls.Game.Move.canceled += OnMove;
        InputManager.Instance.InputControls.Game.Jump.started += OnJump;
    }

    private void OnDisable()
    {
        InputManager.Instance.InputControls.Game.Move.started -= OnMove;
        InputManager.Instance.InputControls.Game.Move.performed -= OnMove;
        InputManager.Instance.InputControls.Game.Move.canceled -= OnMove;
        InputManager.Instance.InputControls.Game.Jump.started -= OnJump;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _gravityPull < 0)
        {
            _gravityPull = 0f;
        }

        Vector3 movementDirection = orientation.right * _moveVector.x  + orientation.forward * _moveVector.z;
        movementDirection.y = 0;

        if (_isGrounded)
        {
            _characterController.Move(moveSpeed * Time.deltaTime * movementDirection.normalized);
        } else
        {
            _characterController.Move(moveSpeed * airMultiplier * Time.deltaTime * movementDirection.normalized);
        }

        _gravityPull += _gravityValue * Time.deltaTime;
        _characterController.Move(_gravityPull * Time.deltaTime * Vector3.up);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 moveVector = ctx.ReadValue<Vector2>().normalized;

        _moveVector = new Vector3(moveVector.x, 0, moveVector.y);

        if (_moveVector.magnitude > 0.2f)
        {
            _animator.SetFloat("moveX", _moveVector.x);
            _animator.SetFloat("moveZ", _moveVector.z);
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!_isGrounded)
        {
            return;
        }

        _gravityPull = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
        _animator.SetTrigger("Jump");
    }
}
