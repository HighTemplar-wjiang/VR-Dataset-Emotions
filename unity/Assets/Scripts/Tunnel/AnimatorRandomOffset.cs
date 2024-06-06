using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorRandomOffset : MonoBehaviour
{
    public bool initRandom = true;
    public float offset;
    public float speed;


    public float speedMin = 0.95f;
    public float speedMax = 1.05f;
    public float offsetMin = 0f;
    public float offsetMax = 1f;


    private float m_offset = 0.0f;
    private float m_speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (initRandom)
        {
            offset = Random.Range(offsetMin, offsetMax);
            speed = Random.Range(speedMin, speedMax);
        }

    }

    // Update is called once per frame
    void Update()
    {

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

    public void reset()
    {
        offset = Random.Range(offsetMin, offsetMax);
        speed = Random.Range(speedMin, speedMax);
    }
}