using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationEventListener : MonoBehaviour
{
    public GameObject locomotionSystem;
    public GameObject player;
    public GameObject sceneController;

    private TeleportationProvider teleportationProvider;

    private void Start()
    {
        teleportationProvider = locomotionSystem.GetComponent<TeleportationProvider>();

        if (teleportationProvider != null)
        {
            teleportationProvider.endLocomotion += OnEndLocomotion;
            Debug.Log("Teleportation provider setup succeeded!");
        }
        else
        {
            Debug.Log("Teleportation provider not found!");
        }
    }

    private void OnDestroy()
    {
        if (teleportationProvider != null)
        {
            teleportationProvider.endLocomotion -= OnEndLocomotion;
        }
    }

    private void OnEndLocomotion(LocomotionSystem system)
    {
        // This method is called when teleportation (or any other locomotion) ends.
        Debug.Log(string.Format("Teleported to {0:s}", player.transform.position.ToString()));
        // Insert any additional logic you want to happen after teleporting here.
    }

    // Triggered with colliders. 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider Event Triggered!");
        if (other.CompareTag("Player"))
        {
            // Triggered when the XR player's collider enters the teleportation pad collider
            HandleTeleportPadInteraction();
        }
    }

    private void HandleTeleportPadInteraction()
    {
        // The XR player interacts with the teleportation pad.
        Debug.Log("Interacted with Teleport Pad!");

        // End the scene and start emotion survey.
        sceneController.GetComponent<SceneController>().StartEmotionSurvey();
    }
}
