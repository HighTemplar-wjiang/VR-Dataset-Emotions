using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtticSoundController : MonoBehaviour
{
    // Start walking time.
    public float startWalkingTime;
    public GameObject man;

    // Set up audio source and timer.
    public GameObject manCreaming;
    public float manCreamingTime;

    // Gun shot time.
    // public float gunshotTime;
    // public GameObject gunShoting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Count time down
        startWalkingTime -= Time.deltaTime;
        manCreamingTime -= Time.deltaTime;
        // gunshotTime -= Time.deltaTime;

        // Start walking.
        if(startWalkingTime <= 0.0f)
        {
            man.SetActive(true);
        }

        // Play creaming sound.
        if(manCreamingTime <= 0.0f)
        {
            manCreaming.SetActive(true);
        }

        // Play gunshot sound.
        // if(gunshotTime <= 0.0f)
        // {
        //     gunShoting.SetActive(true);
        // }

    }
}
