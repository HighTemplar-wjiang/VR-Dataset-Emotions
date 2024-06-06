using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomTeleportationProvider : TeleportationProvider
{
    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        
    }

    private void OnEndLocomotion(LocomotionProvider provider)
    {
        // This method is called when teleportation ends.
        Debug.Log("Teleportation has ended!");
        // Insert any additional logic you want to happen after teleporting here.
    }
}
