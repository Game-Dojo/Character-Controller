using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private string collidingObject;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if(_renderer) _renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collidingObject != "" && !other.CompareTag(collidingObject)) return; 
        onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidingObject != "" && !other.CompareTag(collidingObject)) return;
        onExit?.Invoke();
    }
}
