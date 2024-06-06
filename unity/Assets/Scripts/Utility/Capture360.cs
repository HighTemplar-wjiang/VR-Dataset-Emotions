using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Capture360 : MonoBehaviour
{

    public int imageWidth = 1024;
    public bool saveAsJPEG = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            byte[] bytes = I360Render.Capture(imageWidth, saveAsJPEG);
            if (bytes != null)
            {
                string path = Path.Combine(Application.dataPath, "360render" + (saveAsJPEG ? ".jpeg" : ".png"));
                File.WriteAllBytes(path, bytes);
                Debug.Log("360 render saved to " + path);
            }
        }
    }
}
