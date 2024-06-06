using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;

public class CameraReplayer : MonoBehaviour
{
    // Serializable class representing each recorded pose.
    // Columes: ['id', 'login_id', 'participant_id', 'server_timestamp', 'scene_name',
    //    'position', 'orientation', 'vr_timestamp', 'vr_timestamp_ms',
    //    'position_x', 'position_y', 'position_z', 'orientation_x',
    //    'orientation_y', 'orientation_z', 'orientation_w']
    [System.Serializable]
    public class CameraPoseRecord
    {
        public string id;
        public string login_id;
        public string participant_id;
        public string server_timestamp;
        public string scene_name;
        public string position;
        public string orientation;
        public string vr_timestamp;
        public string vr_timestamp_ms;
        public string position_x;
        public string position_y;
        public string position_z;
        public string orientation_x;
        public string orientation_y;
        public string orientation_z;
        public string orientation_w;
        public double offset_seconds;

    }

    // Camera Rig.
    public GameObject cameraRig;
    public string replayFile;

    // Start is called before the first frame update
    void Start()
    {
        // Read JSON file line by line into array of CameraPoseRecord.
        string[] lines = File.ReadAllLines(replayFile);
        CameraPoseRecord[] records = new CameraPoseRecord[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            records[i] = JsonUtility.FromJson<CameraPoseRecord>(lines[i]);
        }

        // Debug info.
        Debug.Log("Read " + lines.Length + " lines from " + replayFile);

        // Print the first record.
        Debug.Log("First record: " 
            + records[0].id 
            + ", " + records[0].login_id 
            + ", " + records[0].participant_id 
            + ", " + records[0].server_timestamp 
            + ", " + records[0].scene_name + ", " 
            + records[0].position + ", " 
            + records[0].orientation + ", " 
            + records[0].vr_timestamp + ", " 
            + records[0].vr_timestamp_ms + ", " 
            + records[0].position_x + ", " 
            + records[0].position_y + ", " 
            + records[0].position_z + ", " 
            + records[0].orientation_x + ", " 
            + records[0].orientation_y + ", " 
            + records[0].orientation_z + ", " 
            + records[0].orientation_w + ","
            + records[0].offset_seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
