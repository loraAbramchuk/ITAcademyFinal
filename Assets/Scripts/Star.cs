using System;
using UnityEngine;

public class Star : MonoBehaviour
{
    public static event Action StarCollected;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            StarCollected?.Invoke ();
            gameObject.SetActive (false);
        }
    }
}