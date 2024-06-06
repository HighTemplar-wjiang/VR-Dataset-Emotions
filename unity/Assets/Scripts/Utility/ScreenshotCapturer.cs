using UnityEngine;

public class ScreenshotCapturer : MonoBehaviour
{
    public int superSize = 1; // 1 means regular size, 2 would be double, etc.
    public string screenshotFileName = "TreeBillboard.png";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot()
    {
        ScreenCapture.CaptureScreenshot(screenshotFileName, superSize);
        Debug.Log("Screenshot taken: " + screenshotFileName);
    }
}
