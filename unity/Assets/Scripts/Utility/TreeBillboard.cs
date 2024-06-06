using UnityEngine;

public class TreeBillboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Assuming you're using the main camera for rendering
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Adjust the quad's rotation to face the camera. 
        // The LookAt function modifies the transform's rotation so its forward vector points at the target's position.
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, 
                        mainCamera.transform.rotation * Vector3.up);
    }
}
