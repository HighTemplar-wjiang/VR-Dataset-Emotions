using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHairController : MonoBehaviour
{
    public GameObject[] hair;
    public string liamhair;

    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void reset()
    {
        if (hair.Length > 0)
        {
            foreach (GameObject e in hair)
            {
                e.SetActive(false);
            }
            int i = Random.Range(0, hair.Length);
            hair[i].SetActive(true);
            liamhair = hair[i].name;

        }
    }
}
