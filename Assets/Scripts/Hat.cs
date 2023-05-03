using UnityEngine;

public class Hat : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float availableAngle;

    private static float previousTeleportTime;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            if (Time.time - previousTeleportTime > 0.5f)
            {
                Transform thisTransform = transform;
                Vector3 directionToBall = other.transform.position - thisTransform.position;
                float angle = Vector2.Angle (directionToBall, thisTransform.up);

                Debug.Log (angle);

                if (angle <= availableAngle)
                {
                    other.transform.position = target.position;

                    Rigidbody2D ballRigidbody = other.attachedRigidbody;
                    ballRigidbody.velocity = target.up * ballRigidbody.velocity.magnitude;

                    AnchoredJoint2D[] ballJoints = ballRigidbody.GetComponents<AnchoredJoint2D> ();

                    foreach (AnchoredJoint2D joint in ballJoints)
                        joint.enabled = false;

                    previousTeleportTime = Time.time;
                }
            }
        }
    }
}