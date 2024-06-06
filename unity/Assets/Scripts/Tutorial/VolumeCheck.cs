using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class VolumeCheck : MonoBehaviour
{
    // Public variables.
    public Button submitButton; // submit button
    public TMP_InputField inputField; // input field 
    public string verificationCode = "3241"; // Verification code
    public GameObject instructionObject; // Instructions
    public Text message; // Instruction text
    public GameObject rightController; // Right controller object.
    public GameObject teleporationArea; // Teleportation area object.

    // Start is called before the first frame update
    void Start()
    {
        // Button listener.
        submitButton.onClick.AddListener(delegate { SubmitButtonClicked(); });
    }

    // Check if the verification code is correct.
    private void SubmitButtonClicked()
    {
        // Get input code.
        string inputCode = inputField.text;

        // Check if the input code is correct.
        if(inputCode == verificationCode)
        {
            // Hide this object.
            this.gameObject.SetActive(false);
            // Show instruction object.
            instructionObject.SetActive(true);
            // Set right controller XR Ray Interactor to Projectile Curve.
            rightController.GetComponent<XRRayInteractor>().lineType = XRRayInteractor.LineType.ProjectileCurve;
            // Enabling teleportation area.
            teleporationArea.SetActive(true);
        }
        else
        {
            // Show error message.
            message.text = "Incorrect code. Please try again.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
