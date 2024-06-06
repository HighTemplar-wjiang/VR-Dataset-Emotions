using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{

    WaypointCharacterController controller;
    public Animator animator = null;
    public Waypoint currentWaypoint;

    public enum DirectionState
    {
        Forward,
        Backward,
    }

    public DirectionState direction = DirectionState.Forward;

    public bool isRandomDirection = false;

    private void Awake()
    {
        controller = GetComponent<WaypointCharacterController>();

        if (GetComponent<Animator>() != null) { 
            animator = GetComponent<Animator>();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (isRandomDirection) {
            if (0 == Mathf.RoundToInt(Random.Range(0f, 1f)))
            {
                direction = DirectionState.Forward;
            }
            else {
                direction = DirectionState.Backward;
            }
        }
        
        controller.SetDestination(currentWaypoint);

        if (animator != null) {
            animator.SetBool("playWalking", true);
        }

   
    }

    public void setController (WaypointCharacterController controller)
    {
        this.controller = controller;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.reachedDestination && currentWaypoint.isEnd == false)
        {

            animator.SetBool("playWalking", true);
            if (direction == DirectionState.Forward)
            {
                if (currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                {
                    currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];
                }
                else if (currentWaypoint.previousWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    direction = DirectionState.Backward;
                    Debug.Log("WARNING #001: Changed direction");
                }
                else {
                    Debug.Log("ERROR #001: No Directions to pick from");
                }

               
            } else if (direction == DirectionState.Backward) {
                if(currentWaypoint.previousWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }
                else if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                {
                    currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];
                }
                else if (currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = DirectionState.Forward;
                    Debug.Log("WARNING #002: Changed direction");
                }
                else {
                    Debug.Log("ERROR #001: No Directions to pick from");
                }
            }
            controller.SetDestination(currentWaypoint);

            if (currentWaypoint.isReset == true) {
                this.transform.GetComponent<WaypointReset>().reset();
            }

        }
        else if (controller.reachedDestination && currentWaypoint.isEnd == true && controller.despawnOnEnd == true)
        {
            animator.SetBool("playWalking", false);
            Destroy(this.transform.gameObject);
        }
        else if (controller.reachedDestination && currentWaypoint.isEnd == true && controller.despawnOnEnd == false)
        {
            animator.SetBool("playWalking", false);
            Destroy(this);
        }

    }
}
