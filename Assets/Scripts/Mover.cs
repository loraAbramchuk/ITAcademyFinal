using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Transform> destinations;

    private int currentDestinationIndex;

    private void FixedUpdate()
    {
        Transform currentDestination = destinations[currentDestinationIndex];

        if (transform.position != currentDestination.position)
        {
            // Vector3 targetDirection = (currentDestination.position - transform.position).normalized;
            // Vector3 movementStep = targetDirection * speed * Time.deltaTime;
            // float distance = Vector3.Distance(transform.position, currentDestination.position);
            //
            // if (movementStep.magnitude < distance)
            //     transform.position += movementStep;
            // else
            //     transform.position = currentDestination.position;

            transform.position =
                Vector2.MoveTowards(transform.position, currentDestination.position, speed * Time.deltaTime);
        }

        else
        {
            currentDestinationIndex++;

            if (currentDestinationIndex >= destinations.Count)
                currentDestinationIndex = 0;
        }
    }
}