using System;
using System.Collections;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    private Vector3 inputStartPosition;
    private Vector3 inputEndPosition;

    private Camera _camera;
    private Camera Camera
    {
        get
        {
            if (_camera == false)
                _camera = Camera.main;

            if (_camera == false)
                _camera = FindObjectOfType<Camera> ();

            return _camera;
        }
    }

    private void Update ()
    {
        if (Input.GetMouseButtonDown (0))
        {
            inputStartPosition = Camera.ScreenToWorldPoint (Input.mousePosition);
        }

        if (Input.GetMouseButtonUp (0))
        {
            inputEndPosition = Camera.ScreenToWorldPoint (Input.mousePosition);

            Ray ();
        }
    }

    private void Ray ()
    {
        LayerMask layerMask = LayerMask.GetMask ("Link");
        RaycastHit2D hit2D = Physics2D.Linecast (inputStartPosition, inputEndPosition, layerMask);

        if (hit2D.collider)
        {
            AnchoredJoint2D joint2D = hit2D.collider.GetComponent<AnchoredJoint2D> ();

            if (joint2D)
            {
                joint2D.enabled = false;

                DelayedInvoke (4.0f, () =>
                {
                    if (joint2D)
                        joint2D.transform.parent.gameObject.SetActive (false);
                });
            }
        }
    }

    private void DelayedInvoke (float seconds, Action callback)
    {
        StartCoroutine (IE_DelayedInvoke (seconds, callback));
    }

    private static IEnumerator IE_DelayedInvoke (float seconds, Action callback)
    {
        if (seconds > 0)
            yield return new WaitForSeconds (seconds);

        callback?.Invoke ();
    }
}