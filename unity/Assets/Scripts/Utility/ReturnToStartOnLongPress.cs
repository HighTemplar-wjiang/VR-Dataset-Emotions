using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class ReturnToStartOnLongPress : MonoBehaviour
{
    public TextMeshProUGUI debugText; // Reference to a Text GameObject for debugging
    public GameObject rightControllerGameObject; // Reference to the right Controller GameObject
    private XRController rightController; // We'll obtain this from the GameObject in Start()

    public float longPressDuration = 2.0f; // Duration for the long press, in seconds

    private bool isPressed = false; // Whether the AButton is pressed
    private float pressStartTime; // Time when the AButton was first pressed
    private Vector3 startPosition; // Original position of the player (XR Rig)

    private void Start()
    {
        if (rightControllerGameObject == null)
        {
            debugText.text = "Right controller GameObject is not set!";
            Debug.LogError("Right controller GameObject is not set!");
            return;
        }

        // Get the XR Controller component from the GameObject
        rightController = rightControllerGameObject.GetComponent<XRController>();

        if (rightController == null)
        {
            debugText.text = "No XR Controller component found on the Right Controller GameObject!";
            Debug.LogError("No XR Controller component found on the Right Controller GameObject!");
            return;
        }

        startPosition = transform.position; // Assuming this script is attached to XR Rig
    }

    private void Update()
    {
        // Check the "AButton" action's state
        if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed))
        {
            if (pressed && !isPressed)
            {
                // Button was just pressed
                debugText.text = "Button was just pressed";
                Debug.Log("Button was just pressed");
                isPressed = true;
                pressStartTime = Time.time;
            }
            else if (!pressed && isPressed)
            {
                // Button was just released
                isPressed = false;
            }

            if (isPressed && Time.time - pressStartTime > longPressDuration)
            {
                // Button has been pressed for the longPressDuration
                debugText.text = "Button has been pressed for the longPressDuration";
                Debug.Log("Button has been pressed for the longPressDuration");
                transform.position = startPosition; // Reset position to start
                isPressed = false; // Reset the press state to avoid repeated resets
            }
        }
    }
}
