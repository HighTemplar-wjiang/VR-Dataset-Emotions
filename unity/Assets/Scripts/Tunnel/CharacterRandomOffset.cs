using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomOffset : MonoBehaviour
{
    public bool initRandom = true;
    public float offset;
    public float speed;


    private float m_offset = 0.0f;
    private float m_speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (initRandom) { 
            offset = Random.Range(0.0f, 1.0f);
            speed = Random.Range(0.9f, 1.1f);
        }

        if (offset != m_offset)
        {
            m_offset = offset;
            GetComponent<Animator>().SetFloat("Offset", m_offset);
        }
        if (speed != m_speed)
        {
            m_speed = speed;
            GetComponent<Animator>().SetFloat("Speed", m_speed);
        }
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    public void reset()
    {
        offset = Random.Range(0.0f, 1.0f);
        speed = Random.Range(0.9f, 1.1f);
    }
}
