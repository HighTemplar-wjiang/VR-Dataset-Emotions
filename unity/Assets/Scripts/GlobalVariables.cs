using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Global settings.
public static class StudySettings
{
    public static string serverURL;
    public static string loginURL;
    public static string studyDataURL;
    public static int sceneTime; 
}

// Participant data and status.
public static class PlayerData
{
    public static string participantId { get; set; }
    public static string loginId { get; set; }
    public static string[] sceneSequence { get; set; }
    public static int currentSceneIndex { get; set; }

    // static contrsuctor.
    static PlayerData()
    {
        participantId = "1";
        loginId = "-1";
        sceneSequence = new string[1] { "Test" };
        currentSceneIndex = 0;
    }
}

// Network models for JSON body.
public class LoginModel
{
    public string loginId;
    public long timestampUtcUnixMs;
}

// Finish study model.
public class FinishStudyModel
{
    public string participantId;
    public string loginId;
    public long timestampUtcUnixMs;
}

public class LoginResult
{
    public bool result;
    public string message;
    public string[] sequence;
    public string participantId;
}

public class EmotionSurveySingle
{
    public string participantId;
    public string loginId;
    public long timestampUtcUnixMs;
    public string sceneName;
    public float valenceValue;
    public float arousalValue;
    public float dominanceValue;
}


public class GlobalVariables : MonoBehaviour
{
    // UI for global settings.
    public string serverURL;
    public int sceneTime;

    // Start is called before the first frame update
    void Start()
    {
        // Bypass variables to global storage.
        StudySettings.serverURL = this.serverURL;
        StudySettings.loginURL = (this.serverURL + "/login/").Replace("//", "/").Replace(":/", "://");
        StudySettings.studyDataURL = (this.serverURL + "/data/").Replace("//", "/").Replace(":/", "://");
        StudySettings.sceneTime = this.sceneTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
