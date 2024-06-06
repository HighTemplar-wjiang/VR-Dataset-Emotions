using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class LoginEvents : MonoBehaviour
{
    // Variables
    // public string serverURL;

    // Components.
    public TMP_InputField pidInputField;
    public Button submitButton;
    public Text debugInfo;
    private XRInteractorLineVisual[] lineVisuals = new XRInteractorLineVisual[2];

    // Start is called before the first frame update
    void Start()
    {
        // Find line visualizers.
        this.lineVisuals[0] = GameObject.Find("LeftHand Controller").GetComponentInChildren<XRInteractorLineVisual>();
        this.lineVisuals[1] = GameObject.Find("RightHand Controller").GetComponentInChildren<XRInteractorLineVisual>();

        // Event binding.
        pidInputField.onSelect.AddListener(delegate { InputFieldSelected();} );
        pidInputField.onEndEdit.AddListener(delegate { InputFieldUnselected(); });
        submitButton.onClick.AddListener(delegate { StartCoroutine(ButtonClickEvent()); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Invoked when InputField is active.
    private void InputFieldSelected()
    {
        this.lineVisuals[0].enabled = false;
        this.lineVisuals[1].enabled = false;
    }

    // Invoked when finished inputing.
    private void InputFieldUnselected()
    {
        this.lineVisuals[0].enabled = true;
        this.lineVisuals[1].enabled = true;
    }

    // Invoked when submit button clicked.
    private IEnumerator ButtonClickEvent()
    {
        // Construct json file. 
        var participant = new LoginModel();
        participant.loginId = pidInputField.text;
        participant.timestampUtcUnixMs = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
       
        // Serialization. 
        var loginDataSingle = JsonUtility.ToJson(participant);

        // Update UI.
        submitButton.interactable = false;
        submitButton.GetComponentInChildren<Text>().text = "Submitting ...";
        // loadingCircle.SetActive(true);

        // Posting data.
        Debug.Log("[Debug] Login to " + StudySettings.loginURL);
        using (var postRequest = new UnityWebRequest())
        {
            postRequest.url = StudySettings.loginURL; // PostUri is a string containing the url
            // postRequest.url = StudySettings.serverURL; // PostUri is a string containing the url
            postRequest.method = "POST";
            postRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(loginDataSingle)); // postData is Json file as a string
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");
            yield return postRequest.SendWebRequest();

            if (postRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                submitButton.interactable = true;
                submitButton.GetComponentInChildren<Text>().text = "Submit";
                debugInfo.text = "Network error! Please check you WiFi connection.";
                Debug.Log("[Debug] Connection error!");
                Debug.Log(postRequest.error);
            }
            else if (postRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                submitButton.interactable = true;
                submitButton.GetComponentInChildren<Text>().text = "Submit";
                debugInfo.text = "Network error! Please check you WiFi connection.";
                Debug.Log("[Debug] Protocol error!");
                Debug.Log(postRequest.error);
            }
            else
            {
                submitButton.interactable = false;
                submitButton.GetComponentInChildren<Text>().text = "Submitted";
                Debug.Log("[Debug] Post request completed.");
                Debug.Log(postRequest.downloadHandler.text);

                // Parsing returned result. 
                var requestResult = JsonUtility.FromJson<LoginResult>(postRequest.downloadHandler.text);
                debugInfo.text = requestResult.message;

                // Check status.
                if(requestResult.result == false)
                {
                    // Login error.
                    submitButton.interactable = true;
                    submitButton.GetComponentInChildren<Text>().text = "Submit";
                    debugInfo.text = requestResult.message;
                }
                else
                {
                    // Login success.
                    PlayerData.participantId = requestResult.participantId;
                    PlayerData.loginId = participant.loginId;
                    PlayerData.currentSceneIndex = 0;
                    PlayerData.sceneSequence = requestResult.sequence;

                    // Show player data.
                    Debug.Log("[Debug] Participant ID: " + PlayerData.participantId);
                    Debug.Log("[Debug] Login ID: " + PlayerData.loginId);
                    Debug.Log("[Debug] Scene sequence: " + string.Join(", ", PlayerData.sceneSequence));

                    // Load the first scene.
                    SceneManager.LoadScene(sceneName: PlayerData.sceneSequence[PlayerData.currentSceneIndex]);
                }
            }

            // Update UI.
            // loadingCircle.SetActive(false);
        }
    }
}
