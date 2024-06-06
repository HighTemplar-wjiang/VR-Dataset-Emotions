using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class WaypointCharacterController : MonoBehaviour
{
    [ReadOnly] public string destination;
    [ReadOnly] public Vector3 destinationVec;
    Vector3 lastPosition;
    public bool reachedDestination;
    public bool despawnOnEnd = true;
    public float minDistance = 1.5f;
    public float rotationSpeed = 120;
    public float minSpeed = 0.8f, maxSpeed = 1.2f;
    public float movementSpeed;
    Vector3 velocity;

    [ReadOnly] public float destinationDistance = 0.0f;

    private void Start()
    {
        movementSpeed = Random.Range(minSpeed, maxSpeed);
    }
    private void Update()
    {
        if (transform.position != destinationVec)
        {
            Vector3 destinationDirection = destinationVec - transform.position;
            destinationDirection.y = 0;

            destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= minDistance)
            {
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var velocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProduct = Vector3.Dot(transform.right, velocity);
        }
    }

    public void SetDestination(Waypoint destination)
    {
        if (destination != null) { 
            this.destination = destination.getName();
            this.destinationVec = destination.GetPosition();
            reachedDestination = false;
        }
    }
}