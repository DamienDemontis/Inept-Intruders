using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobertMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Movement speed")] private float moveSpeed = 2.0f;
    [SerializeField, Tooltip("Multiplier to apply on movement speed while in the air")] private float airMultiplier = 0.4f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float coyoteTime = 0.5f;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform modelTransform;

    private CharacterController _characterController;
    private Animator _animator;

    private bool _isGrounded = false;
    private float _lastGroundedTime = 0;
    private bool _canJump = true;

    private float _gravityValue = -9.81f;
    private float _gravityPull = 0;

    private bool _canMove = true;
    private Vector3 _moveVector;

    private Vector3 _startingPos = Vector3.zero;

    public Animator Animator => _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = modelTransform.GetComponent<Animator>();

        _startingPos = transform.position;
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
        if (transform.position.y < -10.0f)
        {
            ForcePosition(_startingPos);
        }

        Move();
    }

    public void ForcePosition(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
    }

    private void Move()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded)
        {
            _lastGroundedTime = Time.time;
            _canJump = true;
            _characterController.stepOffset = 0.3f;

            if (_gravityPull < 0)
            {
                _gravityPull = 0f;
            }
        } else
        {
            _characterController.stepOffset = 0.0f;
        }

        if (_canMove)
        {
            Vector3 movementDirection = orientation.right * _moveVector.x  + orientation.forward * _moveVector.z;
            movementDirection.y = 0;

            if (_isGrounded)
            {
                _characterController.Move(moveSpeed * Time.deltaTime * movementDirection.normalized);
            } else
            {
                _characterController.Move(moveSpeed * airMultiplier * Time.deltaTime * movementDirection.normalized);
            }
        }

        _gravityPull += _gravityValue * Time.deltaTime;
        _characterController.Move(_gravityPull * Time.deltaTime * Vector3.up);
    }

    public void ToggleMovement(bool canMove)
    {
        _canMove = canMove;
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
        if (!_canJump)
        {
            return;
        }

        if (!_isGrounded && (Time.time - _lastGroundedTime) > coyoteTime)
        {
            return;
        }

        _canJump = false;

        _gravityPull = Mathf.Sqrt(jumpHeight * -2f * _gravityValue);
        _animator.SetTrigger("Jump");
    }
}
