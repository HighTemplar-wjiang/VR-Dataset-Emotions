using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CameraPoseSender : MonoBehaviour
{
    // Serializable class representing each recorded pose
    [System.Serializable]
    public class CameraPose
    {
        public Vector3 position;
        public Quaternion orientation;
        public string timestamp;
    }

    // Serializable class representing all the data to be sent to the server
    [System.Serializable]
    public class DataToSend
    {
        public string loginId;
        public string participantId;
        public string sceneName;
        public List<CameraPose> cameraPoses = new List<CameraPose>();
    }

    public GameObject cameraRig; // Reference to the camera or XR rig
    public string serverURL = "https://emotionstudy.weiweijiang.xyz/vr-emotion/camera-pose/";

    private List<CameraPose> bufferedPoses = new List<CameraPose>();
    private float timeSinceLastRecord = 0.0f; // Timer for recording poses
    private const float recordInterval = 0.1f; // Time interval for recording poses
    private float timeSinceLastSend = 0.0f;   // Timer for sending data to server
    private const float sendInterval = 3.0f;  // Time interval for sending data

    private void Update()
    {
        // Update timers
        timeSinceLastRecord += Time.deltaTime;
        timeSinceLastSend += Time.deltaTime;

        // Record camera pose if the record interval has passed
        if (timeSinceLastRecord >= recordInterval)
        {
            timeSinceLastRecord = 0.0f;
            RecordCameraPose();
        }

        // Check if it's time to send the buffered data
        if (timeSinceLastSend >= sendInterval)
        {
            timeSinceLastSend = 0.0f;
            SendBufferedDataToServer();
        }
    }

    // Function to record the camera's pose
    private void RecordCameraPose()
    {
        CameraPose pose = new CameraPose
        {
            position = cameraRig.transform.position,
            orientation = cameraRig.transform.rotation,
            timestamp = System.DateTime.UtcNow.ToString("o")  // Using ISO 8601 format for timestamp
        };
        bufferedPoses.Add(pose);
    }

    // Function to send buffered data to the server
    private void SendBufferedDataToServer()
    {
        if (bufferedPoses.Count > 0)
        {
            DataToSend data = new DataToSend
            {
                loginId = PlayerData.loginId,  // Replace with your actual login ID
                participantId = PlayerData.participantId,  // Replace with your actual participant ID
                sceneName = SceneManager.GetActiveScene().name,
                cameraPoses = bufferedPoses
            };

            string jsonData = JsonUtility.ToJson(data);
            StartCoroutine(PostDataCoroutine(jsonData));
            bufferedPoses.Clear();
        }
    }

    // Coroutine for sending a POST request with the given JSON data
    private IEnumerator PostDataCoroutine(string bodyJsonString)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(serverURL, bodyJsonString))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Request failed: {request.error}");
            }
            else if (request.responseCode == 200)
            {
                Debug.Log("Data sent successfully!");
            }
            else
            {
                Debug.LogError($"Server returned response code: {request.responseCode}");
            }
        }
    }

    // Function to be called when the session ends
    public void EndSessionAndSendData()
    {
        if (bufferedPoses.Count > 0)
        {
            SendBufferedDataToServer();
        }
        // Any additional end session logic can be added here
    }
}
