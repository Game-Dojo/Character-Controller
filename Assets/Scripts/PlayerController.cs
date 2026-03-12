using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference moveAction;
    
    [Header("Props")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float rotationSpeed = 60.0f;
    
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

        GroundChecker();
        
        _inputDirection = moveAction.action.ReadValue<Vector3>();
        
        var move = new Vector3(_inputDirection.x, 0, _inputDirection.z);
        if (move != Vector3.zero)
            RotatePlayer(move);
        
        ApplyGravity();
        CheckJump();
        ApplyMovement(move);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var go = hit.collider.gameObject;
        if (go && go.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.AddForce(hit.normal * -5f, ForceMode.Impulse);
        }
    }
    
    #region Movement + Gravity
    private void ApplyGravity()
    {
        _velocity.y += (Gravity * gravityScale) * Time.deltaTime;
    }

    private void ApplyMovement(Vector3 movement)
    {
        var finalMovement = movement * movementSpeed + _velocity;
        _controller.Move(finalMovement * Time.deltaTime);
    }
    
    private void RotatePlayer(Vector3 direction)
    {
        transform.forward = Vector3.Lerp(transform.forward, direction, rotationSpeed*Time.deltaTime);
    }
    
    private void CheckJump()
    {
        if (_isGrounded && jumpAction.action.WasPressedThisFrame())
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * (Gravity * gravityScale));
        }
    }

    private void GroundChecker()
    {
        // Touching ground
        if (_isGrounded && _velocity.y < -1f)
        {
            gravityScale = 1.0f;
            _velocity.y = -1f;
        }
        
        // Falling
        if (!_isGrounded && _velocity.y < 0)
            gravityScale = fallGravityScale;
    }
    #endregion
}
