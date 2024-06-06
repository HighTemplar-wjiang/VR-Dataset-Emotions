using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStates : MonoBehaviour
{
    public string triggerObjectName;

    private Animator animator;

    public GameObject facingTowardObject;
    public bool isFollow = false;

    public float speed = 2;

    private void Awake()
    {
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.name == triggerObjectName)
        {
            animator.SetBool("playShooting", true);
            isFollow = true;
        }
    }


    void Update()
    {
        if (isFollow) {

            Vector3 v = facingTowardObject.transform.position - this.gameObject.transform.position;
            v.y = 0.0f;
            Quaternion q = Quaternion.LookRotation(v);

            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, q, speed * Time.deltaTime);
        }

    }

}
