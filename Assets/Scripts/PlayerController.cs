using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference moveAction;
    
    [Header("Props")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityScale = 2.6f;
    
    private const float Gravity = -16f;
    
    private CharacterController _controller;
    private Vector3 _inputDirection;
    private Vector3 _velocity;
    
    private bool _isGrounded = false;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        
        if (_isGrounded && _velocity.y < -1f)
        {
            gravityScale = 1.0f;
            _velocity.y = -1f;
        }

        if (!_isGrounded && _velocity.y < 0)
            gravityScale = fallGravityScale;
        
        _inputDirection = moveAction.action.ReadValue<Vector3>();
        Vector3 move = new Vector3(_inputDirection.x, 0, _inputDirection.z);
        
        if (move != Vector3.zero)
            transform.forward = move * -1.0f;
        
        _velocity.y += (Gravity * gravityScale) * Time.deltaTime;
        
        if (_isGrounded && jumpAction.action.WasPressedThisFrame())
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * (Gravity * gravityScale));
        }

        var finalMovement = move * movementSpeed + _velocity; //Vector3.up * _velocity.y;
        _controller.Move(finalMovement * Time.deltaTime);
    }
}
