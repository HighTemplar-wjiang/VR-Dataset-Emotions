using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
    //[ReadOnly] public string destination;
    [ReadOnly] public Vector3 destinationVec;
    Vector3 lastPosition;
    public bool reachedDestination;
    public float minDistance = 1.5f;
    public float rotationSpeed;
    public float minSpeed, maxSpeed;
    public float movementSpeed;
    public float movementSpeedAnimationFactor = 1f;
    public Animator animator;
    public DogMotion dogMotion;

    Vector3 velocity;

    [ReadOnly] public float destinationDistance = 0.0f;

    private void Start()
    {
        movementSpeed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        int stateHashCached = Animator.StringToHash("Base Layer.Walking");
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateHashCached) { 
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
                    animator.SetBool("playIdle", true);
                    animator.SetBool("playWalking", true);
                    // animator.SetFloat("speed", movementSpeed * movementSpeedAnimationFactor);
                    dogMotion.tiggerDoneWalking();
                }

                velocity = (transform.position - lastPosition) / Time.deltaTime;
                velocity.y = 0;
                var velocityMagnitude = velocity.magnitude;
                velocity = velocity.normalized;
                var fwdDotProduct = Vector3.Dot(transform.forward, velocity);
                var rightDotProduct = Vector3.Dot(transform.right, velocity);
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destinationVec = destination;
        reachedDestination = false;
    }

    void OnCollisionEnter(Collision collision)
    {
//        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "WalkingArea")
            return;

        reachedDestination = true;
        animator.SetBool("playIdle", true);
        animator.SetBool("playWalking", true);
        // animator.SetFloat("speed", movementSpeed * movementSpeedAnimationFactor);
        dogMotion.tiggerDoneWalking();
    }
}