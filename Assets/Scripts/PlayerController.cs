using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference moveAction;
    
    [Header("Props")]
    [SerializeField] private float cubeSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 10.0f;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    
    private float _gravity = Physics.gravity.y;
    private float _gravityScale = 1.0f;
    
    private bool _isGrounded = false;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        jumpAction.action.started += Jump;
    }
    
    private void OnDisable()
    {
        jumpAction.action.started -= Jump;
    }
   
    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        
        if (_isGrounded && _velocity.y < -2f)
        {
            _gravityScale = 1.0f;
            _velocity.y = -2f;
        }

        if (!_isGrounded && _velocity.y < 0)
            _gravityScale = 2.5f;
        
        _moveDirection = moveAction.action.ReadValue<Vector3>();
        Vector3 move = new Vector3(_moveDirection.x, 0, _moveDirection.z);
        move = Vector3.ClampMagnitude(move, 1f);
        
        if (move != Vector3.zero)
            transform.forward = move * -1.0f;
        
        _velocity.y += (_gravity * _gravityScale) * Time.deltaTime;
        
        if (_isGrounded && jumpAction.action.WasPressedThisFrame())
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * (_gravity * _gravityScale));
        }

        
        var finalMovement = move * cubeSpeed + Vector3.up * _velocity.y;
        _controller.Move(finalMovement * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (_isGrounded) return;
        print("JUMP");
        _moveDirection.y += Mathf.Sqrt(jumpHeight * -2.0f * (_gravity * _gravityScale));
    }
}
