using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllegalMoveHandler : MonoBehaviour
{
    public GameObject playerCam;
    public AudioClip wow;
    private AudioSource[] sound;
    private bool illegal = false;
    private bool soundOn = false;
    public Text debugText;
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("GameObject Hit: " + col.gameObject.name);
    }

    void HorizontalMove()
    {
        float degreeY = playerCam.transform.localRotation.eulerAngles.y;
        float degreeX = playerCam.transform.localRotation.eulerAngles.x;
        float positionZ = playerCam.transform.localPosition.z;

        //Debug.Log(positionZ);
        illegal = ((degreeY >= 30 && degreeY <= 330) || 
            (positionZ >= 0.8) || (positionZ < 0.1) ||
            (positionZ <= 0.6 && degreeX >= 50 && degreeX <= 330)) ? true : false; 
    }

    void Update()
    {
        HorizontalMove();
        if (illegal && !soundOn)
        {
            sound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();
            sound[2].PlayOneShot(wow, 0.3f);
            //Debug.Log("illegal");
            debugText.text = "Illegal";
            soundOn = true;
        }
        if (!illegal)
        {
            debugText.text = "Legal";
            soundOn = false;
        }
    }
}
