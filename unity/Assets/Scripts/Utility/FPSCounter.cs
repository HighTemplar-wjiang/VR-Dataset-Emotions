using UnityEngine;
using TMPro;  // Namespace for TextMeshPro

public class FPSCounter : MonoBehaviour
{
    // Reference to the TextMeshProUGUI component to display FPS.
    public TextMeshProUGUI fpsText;

    // The rate (in seconds) at which the FPS text is updated.
    // For instance, if it's set to 0.5, the FPS will update every half second.
    public float updateRateSeconds = 0.5f; 

    // Variables to keep track of frames and time.
    private int frameCount;
    private float deltaTime = 0.0f;
    private float fps;
    private float nextUpdate;

    private void Start()
    {
        // Set the initial next update time.
        nextUpdate = Time.time;
    }

    private void Update()
    {
        // Increment the frame count and accumulate delta time.
        frameCount++;
        deltaTime += Time.unscaledDeltaTime;

        // If the current time is beyond the next scheduled update...
        if (Time.time > nextUpdate)
        {
            // Update the next scheduled update time.
            nextUpdate += updateRateSeconds;

            // Calculate the frames per second.
            fps = frameCount / deltaTime;

            // Reset counters for the next interval.
            frameCount = 0;
            deltaTime = 0;

            // Display the calculated FPS on the TextMeshProUGUI element.
            fpsText.text = string.Format("FPS: {0:0}", fps);
        }
    }
}
