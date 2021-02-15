using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    private Animator _animator;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    
    private Transform _playerTransform; 
    
    private Vector2 _inputVector = Vector2.zero;
    private Vector3 _moveDirection = Vector2.zero;
    
    private readonly int _movementXHash = Animator.StringToHash("MovementX");
    private readonly int _movementYHash = Animator.StringToHash("MovementZ");
    private readonly int _isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int _isJumpingHash = Animator.StringToHash("IsJumping");

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
        _playerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        
        if (_playerController.isJumping) return;
        if (!(_inputVector.magnitude > 0)) _moveDirection = Vector3.zero;
        
        _moveDirection = _playerTransform.forward * _inputVector.y + _playerTransform.right * _inputVector.x;
        
        float currentSpeed = _playerController.isRunning ? runSpeed : walkSpeed;
        
        Vector3 movementDirection = _moveDirection * (currentSpeed * Time.deltaTime);
        
        _playerTransform.position += movementDirection;

    }

    public void OnMovement(InputValue value)
    {
        //Debug.Log(value.Get<Vector2>());
        _inputVector = value.Get<Vector2>();
        
        _animator.SetFloat(_movementXHash, _inputVector.x);
        _animator.SetFloat(_movementYHash, _inputVector.y);
    }
    
    public void OnRun(InputValue value)
    {
        //Debug.Log(value.isPressed);
        _playerController.isRunning = value.isPressed;
        _animator.SetBool(_isRunningHash, value.isPressed);
    }
    
    public void OnJump(InputValue value)
    {
        _playerController.isJumping = value.isPressed;
        _animator.SetBool(_isJumpingHash, value.isPressed);
            
        _rigidbody.AddForce((_playerTransform.up + _moveDirection) * jumpForce, ForceMode.Impulse);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") && !_playerController.isJumping) return;

        _playerController.isJumping = false;
        _animator.SetBool(_isJumpingHash, false);
    }

}
