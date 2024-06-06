using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorOnCharacter : MonoBehaviour
{
    public Animator myAnimatorController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            myAnimatorController.SetBool("openDoor", true);
        }
    }
}
