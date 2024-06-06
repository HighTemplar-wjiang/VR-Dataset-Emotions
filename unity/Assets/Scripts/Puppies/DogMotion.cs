using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DogMotion : MonoBehaviour
{
    public GameObject walkingArea;

    public Animator animator;
    System.Random rnd = new System.Random();

    public enum STATE { none, easy, difficult };

    private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    private double lastChange = 0;
    private double now;
    public long cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        now = (DateTime.Now.ToUniversalTime() - epochStart).TotalMilliseconds;
    }

    // Update is called once per frame
    void Update()
    {
        now = (DateTime.Now.ToUniversalTime() - epochStart).TotalMilliseconds;
        if ((now - lastChange) - cooldown >= 0)
        {

            animator.SetBool("playSitting", false);
            animator.SetBool("playSittingDown", false);
            animator.SetBool("playGettingUp", false);
            animator.SetBool("playIdle", false);
            animator.SetBool("playWalking", false);

            float r = UnityEngine.Random.Range(0.0f, 1.0f);
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Sitting Down"))
            {
                animator.SetBool("playSitting", true);
                cooldown = (long)UnityEngine.Random.Range(1000f, 5000f);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Sitting"))
            {
                animator.SetBool("playGettingUp", true);
                cooldown = 300;
                animator.SetFloat("timeUp", (float)cooldown);
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Getting Up"))
            {
                animator.SetBool("playIdle", true);
                cooldown = (long)UnityEngine.Random.Range(500f, 5000f); ;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Idle") | animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash("Base Layer.Walking"))
            {
                if (r > .5)
                {
                    //Debug.Log("playWalking");
                    animator.SetBool("playWalking", true);
                    cooldown = 10000000000;

                    Vector3 v = getPointInSpawnArea();
                    CharacterNavigationController cnc = transform.GetComponent<CharacterNavigationController>();
                    cnc.SetDestination(v);

                }
                else if (r <= .4 && r > .2)
                {
                    animator.SetBool("playSittingDown", true);
                    cooldown = 200;
                    animator.SetFloat("timeDown", (float) cooldown);
                }
                else
                {
                    animator.SetBool("playIdle", true);
                    cooldown = (long)UnityEngine.Random.Range(500f, 5000f);
                }
            }
            else {
                Debug.LogError("Not a valid Animation State");
            }
            lastChange = (DateTime.Now.ToUniversalTime() - epochStart).TotalMilliseconds;
        }
    }

    internal void tiggerDoneWalking()
    {
        cooldown = 0;
    }

    private Vector3 getPointInSpawnArea()
    {
        Vector3 point;


        Vector3 p = walkingArea.transform.position;
        Vector3 s = walkingArea.transform.localScale;

        Bounds bounds = new Bounds(p, s/2);

        //Debug.Log(bounds.min.x + " - " + bounds.max.x);

        Vector3 tempPoint = new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
        return tempPoint;
    }

    public static bool IsPointWithinCollider(Collider collider, Vector3 point)
    {
        return collider.ClosestPoint(point) == point;
    }
}
