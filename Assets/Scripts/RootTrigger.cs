using UnityEngine;

public class RootTrigger : MonoBehaviour
{
    [SerializeField] private ConnectionCreator connectionCreator;
    [SerializeField] private float connectionLength = 1.2f;

    private Collider2D _collider2D;
    private Rigidbody2D _rigidbody2D;

    private void Start ()
    {
        _rigidbody2D = GetComponent<Rigidbody2D> ();
        _collider2D = GetComponent<Collider2D> ();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            connectionCreator.CreateConnection (_rigidbody2D, other.attachedRigidbody, connectionLength);
            _collider2D.enabled = false;
        }
    }
}