using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneController : MonoBehaviour
{

    // Variables.
    public float countdownTimerSeconds = -1;
    // public GameObject surveyCanvas;
    public GameObject doorObject;
    // If it is tutorial scene.
    public bool skipSurvey = false;

    // Door animation.
    private Animator doorAnimator;

    // Arrow.
    public GameObject arrowObject;

    // Scene status.
    private bool sceneTimeUp = false;

    // Start is called before the first frame update
    void Start()
    {
        // Skip in tutorial scene.
        if(skipSurvey == false)
        {
            // Use global settings.
            if(this.countdownTimerSeconds < 0)
            {
                this.countdownTimerSeconds = StudySettings.sceneTime;
            }

            // Get door animator.
            this.doorAnimator = this.doorObject.GetComponent<Animator>();   
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(sceneTimeUp == true)
        {
            // Player should exit the scene. 
            // StartEmotionSurvey();
        }
        else
        {
            // Count down.
            countdownTimerSeconds -= Time.deltaTime;
            // Debug.Log(string.Format("[Timer] {0}", countdownTimerSeconds));

            // Time up.
            if (countdownTimerSeconds <= 0.0f)
            {
                if (skipSurvey == false)
                {
                    // Time up.
                    sceneTimeUp = true;
                    // surveyCanvas.SetActive(true);
                    // SceneManager.LoadScene("EmotionSurvey");
                    // gameObject.SendMessage("StartEmotionSurvey");
                    this.doorAnimator.SetBool("openExitDoor", true);
                    this.arrowObject.SetActive(true);

                    Debug.Log("Scene time up, openning the door.");
                }
            }
        }
    }

    // Start emotion survey.
    public void StartEmotionSurvey()
    {
        if (skipSurvey == true)
        {

        }
        else
        {
            // Set camera pose data.
            gameObject.GetComponent<CameraPoseSender>().EndSessionAndSendData();

            // Load emotion survey scene.
            SceneManager.LoadScene("EmotionSurvey");
        }
    }
}
