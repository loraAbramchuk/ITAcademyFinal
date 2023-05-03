using UnityEngine;

[RequireComponent (typeof (AnchoredJoint2D))]
[RequireComponent (typeof (CircleCollider2D))]
public class Link : MonoBehaviour
{
    [Space]
    [SerializeField] private AnchoredJoint2D anchoredJoint2D;
    [SerializeField] private CircleCollider2D circleCollider2D;

    public AnchoredJoint2D AnchoredJoint2D
    {
        get
        {
            if (anchoredJoint2D == false)
                anchoredJoint2D = GetComponent<AnchoredJoint2D> ();

            return anchoredJoint2D;
        }
    }

    public CircleCollider2D CircleCollider2D
    {
        get
        {
            if (circleCollider2D == false)
                circleCollider2D = GetComponent<CircleCollider2D> ();

            return circleCollider2D;
        }
    }
}