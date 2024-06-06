using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

// public class EmotionSurveySingle
// {
//     public string participantId;
//     public string loginId;
//     public long timestampUtcUnixMs;
//     public string sceneName;
//     public float valenceValue;
//     public float arousalValue;
// }

public class SurveyEvents : MonoBehaviour
{
    // Variables
    public string serverURL;

    // Components 
    public Slider valenceSlider;
    public Slider arousalSlider;
    public Button submitButton;
    public Text debugInfo;
    // public GameObject loadingCircle;

    // Input checker.
    private bool valenceChanged = false;
    private bool arousalChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        // Debug info.
        Debug.Log("[Debug] Survey start");

        // Add listener to slider. 
        valenceSlider.onValueChanged.AddListener(delegate { ValenceValueChangeEvent(); });
        arousalSlider.onValueChanged.AddListener(delegate { ArousalValueChangeEvent(); });

        // Add listener to submit button.
        submitButton.onClick.AddListener(delegate { StartCoroutine(ButtonClickEvent()); });

        // Hide loading circle.
        // loadingCircle.SetActive(false);
    }

    // Invoked when slider value changed. 
    private void ValenceValueChangeEvent()
    {
        // Debug info.
        // Debug.Log("[Debug] Slider event");

        System.String debugString = System.String.Format(
            "Valence: {0:f2}, arousal: {1:f2}", valenceSlider.value, arousalSlider.value);
        // debugInfo.text = debugString;

        // Flag changed.
        valenceChanged = true;
    }

    // Invoked when slider value changed. 
    private void ArousalValueChangeEvent()
    {
        // Debug info.
        // Debug.Log("[Debug] Slider event");

        System.String debugString = System.String.Format(
            "Valence: {0:f2}, arousal: {1:f2}", valenceSlider.value, arousalSlider.value);
        // debugInfo.text = debugString;

        // Flag changed.
        arousalChanged = true;
    }

    // Invoked when submit button clicked.
    private IEnumerator ButtonClickEvent()
    {
        // Check if answered. 
        if ((valenceChanged == false) && (arousalChanged == false))
        {
            debugInfo.text = "Please rate your pleasure and arousal.";
            yield break;
        }

        // Construct json file. 
        var surveySingle = new EmotionSurveySingle();
        surveySingle.participantId = PlayerData.participantId;
        surveySingle.loginId = PlayerData.loginId;
        surveySingle.timestampUtcUnixMs = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        surveySingle.sceneName = SceneManager.GetActiveScene().name;
        surveySingle.valenceValue = valenceSlider.value;
        surveySingle.arousalValue = arousalSlider.value;

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
            postRequest.url = serverURL; // PostUri is a string containing the url
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
                debugInfo.text = "[Error] Please check your Internet connection.";
            }
            else if(postRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                submitButton.interactable = true;
                submitButton.GetComponentInChildren<Text>().text = "Submit";
                Debug.Log("[Debug] Protocol error!");
                debugInfo.text = "[Error] Please check your Internet connection.";
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
