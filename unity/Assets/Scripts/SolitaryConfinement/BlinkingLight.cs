using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public float minOnTime = 0.1f;      // Minimum time (in seconds) light stays on.
    public float maxOnTime = 1.0f;      // Maximum time (in seconds) light stays on.
    public float minOffTime = 0.1f;     // Minimum time (in seconds) light stays off.
    public float maxOffTime = 1.0f;     // Maximum time (in seconds) light stays off.
    private float nextSwitchTime;
    private Light myLight;

    private void Start()
    {
        myLight = GetComponent<Light>();
        SetRandomSwitchTime();
    }

    private void Update()
    {
        if (Time.time >= nextSwitchTime)
        {
            myLight.enabled = !myLight.enabled; // Toggle light state
            SetRandomSwitchTime();
        }
    }

    private void SetRandomSwitchTime()
    {
        float randomTime = myLight.enabled ? 
            Random.Range(minOffTime, maxOffTime) : 
            Random.Range(minOnTime, maxOnTime);
            
        nextSwitchTime = Time.time + randomTime;
    }
}
