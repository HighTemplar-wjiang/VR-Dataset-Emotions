using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[ExecuteInEditMode]
public class DuplicateObjectAlongLine : MonoBehaviour
{
    public GameObject myObject;
    public int count;
    public float length = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        Debug.Log("TEST");
        foreach (Transform child in this.transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        foreach (int value in Enumerable.Range(1, count))
        {
            Instantiate(myObject, new Vector3 (myObject.transform.position.x, myObject.transform.position.y , myObject.transform.position.z + value * length), Quaternion.identity, this.transform);

        }
    }
}
