using UnityEngine;

public class PinguinController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private bool _playerIsInRange = false;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_playerIsInRange) return;
        AvoidPlayer();
    }

    private void AvoidPlayer()
    {
        Vector3 forwardDirection = (player.transform.position - transform.position).normalized;
        _rb.AddForce(forwardDirection * -1f, ForceMode.Impulse);
    }

    public void IsPlayerInRange(bool state)
    {
        _playerIsInRange = state;
    }
}
