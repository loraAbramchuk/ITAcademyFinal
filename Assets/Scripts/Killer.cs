using System;
using UnityEngine;

[RequireComponent (typeof (Collider2D))]
public class Killer : MonoBehaviour
{
    public static event Action PlayerKilledEvent;

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            other.gameObject.SetActive (false);
            PlayerKilledEvent?.Invoke ();
        }
    }
}