using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Data class to be send as JSON.
public class InteractionData
{
    public string participantId;
    public long timestampUtcUnixMs;
    public string sceneName;
    public Vector3 position;
    public Vector3 rotationDegrees;
}


public class InteractionEvents : MonoBehaviour
{
    // Variables.
    public GameObject mainCamera;
    public string serverURL;

    // Start is called before the first frame update
    void Start()
    {
        // Get camera transform periodically. 
        InvokeRepeating("GetXRTransform", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get current XR headset transform and send to server. 
    void GetXRTransform()
    {
        // Get the transform from attributes. 
        var position = mainCamera.transform.position;
        var rotation = mainCamera.transform.eulerAngles;
        
        // Debug info.
        Debug.Log(System.String.Format(
            "[Debug] Camera transform: {0:s}, {1:s}", 
            position.ToString("F2"), 
            rotation.ToString("F2")));

        // Construct json file. 
        /*
        var single = new EmotionSurveySingle();
        surveySingle.participantId = PlayerData.participantId;
        surveySingle.timestampUtcUnixMs = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        surveySingle.sceneName = SceneManager.GetActiveScene().name;
        surveySingle.valenceValue = valenceSlider.value;
        surveySingle.arousalValue = arousalSlider.value;
        */
        // TODO: Send the data to server.

    }

    // TODO: Send data to server.
    /*
    private IEnumerator PostInteractionData(InteractionData interactionData)
    {
        // Serialization. 
        var surveyDataSingle = JsonUtility.ToJson(surveySingle);

        // Update UI.
        submitButton.interactable = false;
        submitButton.GetComponentInChildren<Text>().text = "Submitting ...";
        // loadingCircle.SetActive(true);

        // Posting data.
        Debug.Log("[Debug] Posting survey result...");
        using (var postRequest = new UnityWebRequest())
        {
            postRequest.url = StudySettings.serverURL; // PostUri is a string containing the url
            postRequest.method = "POST";
            postRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(surveyDataSingle)); // postData is Json file as a string
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");
            yield return postRequest.SendWebRequest();

            if(postRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                submitButton.interactable = true;
                submitButton.GetComponentInChildren<Text>().text = "Submit";
                Debug.Log("[Debug] Connection error!");
            }
            else if(postRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                submitButton.interactable = true;
                submitButton.GetComponentInChildren<Text>().text = "Submit";
                Debug.Log("[Debug] Protocol error!");
            }
            else
            {
                submitButton.interactable = false;
                submitButton.GetComponentInChildren<Text>().text = "Submitted";
                Debug.Log("[Debug] Post request completed.");

                if(PlayerData.currentSceneIndex < PlayerData.sceneSequence.Length - 1)
                {
                    // Load next scene.
                    PlayerData.currentSceneIndex++;
                    SceneManager.LoadScene(sceneName: PlayerData.sceneSequence[PlayerData.currentSceneIndex]);
                }
                else
                {
                    // End of study.
                    SceneManager.LoadScene("EndScene");
                }
            }

            // Update UI.
            // loadingCircle.SetActive(false);
            
        }
    }*/


}
