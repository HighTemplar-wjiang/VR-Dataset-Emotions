using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    public bool isStart;
    public bool isEnd;
    public bool isReset;

    [Range(0f, 20f)]
    public float width = 1f;

    public List<Waypoint> branches = new List<Waypoint>();

    [Range(0f, 1f)]
    public float branchRatio = 0.5f;


    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }


    public string getName()
    {
        return transform.name;
    }

    public List<Waypoint> getAllNextWaypoints()
    {
        if (branches.Count == 0)
        {
            List<Waypoint> ret = new List<Waypoint>();
            ret.Add(this.nextWaypoint);
            return ret;
        }
        else
        {
            return branches;
        }
    }
}
