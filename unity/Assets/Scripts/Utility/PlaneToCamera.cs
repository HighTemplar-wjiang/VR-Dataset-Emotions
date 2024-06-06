using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneToCamera : MonoBehaviour
{
    public Camera targetCamera;
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        // Set plane face to camera forward.
        plane.transform.forward = targetCamera.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
