using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static event Action<int> TargetReachedEvent;

    private int starsCounter;

    private void Awake ()
    {
        Star.StarCollected += Star_OnStarCollected;
    }

    private void OnDestroy ()
    {
        Star.StarCollected -= Star_OnStarCollected;
    }

    private void Star_OnStarCollected ()
    {
        starsCounter++;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            other.gameObject.SetActive (false);
            TargetReachedEvent?.Invoke (starsCounter);
        }
    }
}