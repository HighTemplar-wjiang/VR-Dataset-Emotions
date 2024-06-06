using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class FinishStudy : MonoBehaviour
{
    public string serverURL = "https://emotionstudy.weiweijiang.xyz/vr-emotion/finish-study/";
    public Button submitButton;
    public TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Start()
    {
        // Button listener.
        submitButton.onClick.AddListener(delegate { SubmitButtonClickEvent(); });

        // Send finish study request.
        SendFinishStudy();
    }

    void SubmitButtonClickEvent()
    {
        // Send finish study request.
        SendFinishStudy();
    }

    // Post to the server to finish the study.
    public void SendFinishStudy()
    {
        // Get participant id.
        string participantId = PlayerData.participantId;
        string loginId = PlayerData.loginId;

        // Create finish study model.
        FinishStudyModel finishStudyModel = new FinishStudyModel();
        finishStudyModel.participantId = participantId;
        finishStudyModel.loginId = loginId;
        finishStudyModel.timestampUtcUnixMs = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Post to the server.
        StartCoroutine(PostFinishStudy(finishStudyModel));
    }

    // Post to the server.
    private IEnumerator PostFinishStudy(FinishStudyModel finishStudyModel)
    {
        // Convert to json.
        string json = JsonUtility.ToJson(finishStudyModel);
        Debug.Log(string.Format("[Debug] Finish study json: {0}", json));

        // Post to the server.
        using (UnityWebRequest request = UnityWebRequest.Post(serverURL, json))
        {
            // Set request header.
            request.SetRequestHeader("Content-Type", "application/json");

            // Send request.
            yield return request.SendWebRequest();

            // Check response.
            if (UnityWebRequest.Result.ConnectionError == request.result)
            {
                messageText.text = "Error! Please check your internet connection!";
                Debug.Log(request.error);
            }
            else if (UnityWebRequest.Result.ProtocolError == request.result)
            {
                messageText.text = "Error! Please check your internet connection!";
                Debug.Log(request.error);
            }
            else
            {
                messageText.text = "Done! You can now exit the VR app.";
                Debug.Log(string.Format("[Debug] Finish study response: {0}", request.downloadHandler.text));
                submitButton.interactable = false;
                submitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Done";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
