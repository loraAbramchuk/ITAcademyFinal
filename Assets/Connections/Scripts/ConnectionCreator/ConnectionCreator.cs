using System;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionCreator : MonoBehaviour
{
    [Space]
    [SerializeField] private ConnectionCreator other;
    public ConnectionCreator Other => other;

    [Space]
    [SerializeField] private Link linkAsset;

    [Space]
    [SerializeField] private bool needBaseLink;

    [Space]
    [SerializeField, NonReorderable] private List<Connection> connections;

    public void Reconnect ()
    {
        foreach (Connection connection in connections)
        {
            if (connection?.Reconnect ?? false)
                CreateConnection (connection.Root, connection.Target, connection.Distance);
        }
    }

    public void CopyFromOther () =>
        CopyFromOther (Other);

    // ReSharper disable once MemberCanBePrivate.Global;
    public void CopyFromOther (ConnectionCreator other)
    {
        if (other &&
            other != this)
        {
            linkAsset = other.linkAsset;
            needBaseLink = other.needBaseLink;

            connections = new List<Connection> ();

            foreach (Connection otherConnection in other.connections)
            {
                if (otherConnection != null)
                    connections.Add (new Connection (otherConnection));
            }

            SetDirty (this);
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global;
    public void CreateConnection (Rigidbody2D root, Rigidbody2D target, float distance)
    {
        DestroyConnection (root, target);

        Transform rootTransform = root.transform;

        Vector3 rootPosition = rootTransform.position;
        Vector3 targetPosition = target.transform.position;

        Vector3 directionToRoot = (rootPosition - targetPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation (Vector3.forward, directionToRoot);

        float localRadius = linkAsset.CircleCollider2D.radius;
        float lossyRadius = localRadius * rootTransform.lossyScale.x;

        int count = (int) (distance / (lossyRadius * 2)) - 1;
        float positionOffset = Vector2.Distance (rootPosition, targetPosition) / (count + 1);

        Link previousLink = null;
        int index = 0;

        if (needBaseLink)
        {
            Link baseLink = Instantiate (linkAsset, rootTransform);

            baseLink.name = $"{root.name} {linkAsset.name} (base)";
            Transform linkTransform = baseLink.transform;

            linkTransform.position = rootPosition;
            linkTransform.rotation = rotation;

            PrepareJoint (baseLink.AnchoredJoint2D, root, 0);

            previousLink = baseLink;
        }

        while (index < count)
        {
            Link link = Instantiate (linkAsset, rootTransform);

            link.name = $"{root.name} {linkAsset.name} ({index})";
            Transform linkTransform = link.transform;

            linkTransform.position = (previousLink ? previousLink.transform.position : rootPosition) - directionToRoot * positionOffset;
            linkTransform.rotation = rotation;

            PrepareJoint (link.AnchoredJoint2D, previousLink ? previousLink.AnchoredJoint2D.attachedRigidbody : root, lossyRadius * 2);

            previousLink = link;
            index++;
        }

        Type jointType = linkAsset.AnchoredJoint2D.GetType ();
        Component targetJoint = target.gameObject.AddComponent (jointType);

        PrepareJoint (targetJoint as AnchoredJoint2D, previousLink ? previousLink.AnchoredJoint2D.attachedRigidbody : root, lossyRadius * 2);

        SetDirty (root.gameObject);
        SetDirty (target.gameObject);
    }

    private void PrepareJoint (AnchoredJoint2D anchoredJoint2D, Rigidbody2D connectedBody, float distance)
    {
        anchoredJoint2D.autoConfigureConnectedAnchor = false;
        anchoredJoint2D.connectedBody = connectedBody;
        anchoredJoint2D.connectedAnchor = Vector2.zero;
        anchoredJoint2D.enableCollision = true;

        switch (anchoredJoint2D)
        {
            case HingeJoint2D hingeJoint2D:
            {
                hingeJoint2D.connectedAnchor = Vector2.down * (distance / anchoredJoint2D.transform.lossyScale.x);
                break;
            }

            case DistanceJoint2D distanceJoint2D:
            {
                distanceJoint2D.autoConfigureDistance = false;
                distanceJoint2D.distance = distance;

                distanceJoint2D.maxDistanceOnly = true;

                break;
            }

            case SpringJoint2D springJoint2D:
            {
                springJoint2D.autoConfigureDistance = false;
                springJoint2D.distance = distance;

                if (linkAsset.AnchoredJoint2D is SpringJoint2D linkSpringJoint2D)
                {
                    springJoint2D.dampingRatio = linkSpringJoint2D.dampingRatio;
                    springJoint2D.frequency = linkSpringJoint2D.frequency;
                }

                break;
            }
        }
    }

    private static void DestroyConnection (Rigidbody2D root, Rigidbody2D target)
    {
        var rootJoints = root.gameObject.GetComponentsInChildren<AnchoredJoint2D> (true);
        var targetJoints = new List<AnchoredJoint2D> (target.gameObject.GetComponents<AnchoredJoint2D> ());

        foreach (var rootJoint in rootJoints)
        {
            if (rootJoint)
            {
                if (targetJoints.Count > 0)
                {
                    foreach (var targetJoint in targetJoints.ToArray ())
                    {
                        if (targetJoint)
                        {
                            if (targetJoint.connectedBody == false ||
                                targetJoint.connectedBody == rootJoint.attachedRigidbody)
                            {
                                targetJoints.Remove (targetJoint);

                                if (Application.isPlaying)
                                    Destroy (targetJoint);
                                else
                                    DestroyImmediate (targetJoint);
                            }
                        }
                    }
                }

                if (Application.isPlaying)
                    Destroy (rootJoint.gameObject);
                else
                    DestroyImmediate (rootJoint.gameObject);
            }
        }

        if (targetJoints.Count > 0)
        {
            foreach (var targetJoint in targetJoints.ToArray ())
            {
                if (targetJoint)
                {
                    if (targetJoint.connectedBody == false ||
                        targetJoint.connectedBody == root)
                    {
                        targetJoints.Remove (targetJoint);

                        if (Application.isPlaying)
                            Destroy (targetJoint);
                        else
                            DestroyImmediate (targetJoint);
                    }
                }
            }
        }
    }

    private static void SetDirty (UnityEngine.Object target)
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            if (target)
                UnityEditor.EditorUtility.SetDirty (target);
        }
#endif
    }

    [Serializable]
    protected class Connection
    {
        [Space]
        [SerializeField] private Rigidbody2D root;
        [SerializeField] private Rigidbody2D target;
        [SerializeField] private float distance;
        [SerializeField] private bool reconnect;

        public Connection (Connection other)
        {
            root = other.root;
            target = other.target;
            distance = other.distance;
            reconnect = other.reconnect;
        }

        public Rigidbody2D Root => root;
        public Rigidbody2D Target => target;
        public float Distance => distance;
        public bool Reconnect => reconnect;
    }
}