using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class RadioButtonEvents : MonoBehaviour
{
    // Toggle button referece.
    private ToggleGroup toggleGroup;

    // Record value.
    public bool valenceChanged { get; set; }
    public string selectedToggleName { get; set; }
    public int selectedToggleValue { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        // Set default values.
        valenceChanged = false;
        toggleGroup = GetComponent<ToggleGroup>();

        // Add listener to every toggle button.
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate {
                //GetSelectedToggle();
                ToggleChanged(toggle);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        // GetSelectedToggle();
    }

    // Toggle button listener.
    void ToggleChanged(Toggle toggle)
    {
        // Check if toggle is On.
        if (toggle.isOn)
        {
            // Turn of allow switch off.
            toggle.group.allowSwitchOff = false;
            valenceChanged = true;

            // Show the name of the toggle.
            // Debug.Log("Toggle is On" + toggle.name);

            // Get the name and value of the selected toggle.
            selectedToggleName = toggle.name;
            selectedToggleValue = Variables.Object(toggle).Get<int>("value");

            // Show the name and value of the selected toggle.
            Debug.Log("Selected Toggle Name: " + selectedToggleName);
            Debug.Log("Selected Toggle Value: " + selectedToggleValue);
        }
        else
        {
            // Toggle is Off.
            // Debug.Log("Toggle is Off" + toggle.name);
        }
    }

    // Toggle group listener.
    void GetSelectedToggle()
    {
        Toggle[] toggles = GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                Debug.Log(toggle.name);
            }
        }
    }
}
