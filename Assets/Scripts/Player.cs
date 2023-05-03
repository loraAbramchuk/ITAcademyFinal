using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body2D;
    [SerializeField] private float force;

    private void Start ()
    {
        body2D.AddForce (Vector2.left * force);
    }
}