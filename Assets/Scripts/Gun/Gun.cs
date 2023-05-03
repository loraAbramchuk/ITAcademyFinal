using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float forseGun;

    private Rigidbody2D attachedRigidbody;

    public void OnPointerClick (PointerEventData eventData)
    {
        if (attachedRigidbody)
            attachedRigidbody.AddForce (transform.up * forseGun);
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
            attachedRigidbody = other.attachedRigidbody;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
            attachedRigidbody = null;
    }
}