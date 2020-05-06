using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/****************************** 
Script for illegal actions
******************************/

public class IllegalMoveHandler : MonoBehaviour
{
    [HideInInspector]
    public bool illegal = false;

    private GameObject playerCam;
    private AudioSource[] sound;
    private bool soundOn = false;
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("MainCamera");
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    // Function for illegal camera position and rotation
    void CameraMove()
    {
        float degreeY = playerCam.transform.localRotation.eulerAngles.y;
        float degreeX = playerCam.transform.localRotation.eulerAngles.x;
        float positionZ = playerCam.transform.localPosition.z;

        illegal = ((degreeY >= 30 && degreeY <= 330) ||
            (positionZ >= 0.8) || (positionZ < 0.1) ||
            (positionZ <= 0.6 && degreeX >= 50 && degreeX <= 330)) ? true : false;
    }

    void Update()
    {
        // If starting and illegal detect is set
        if (!setting.onPrepare && setting.illegalDetect)
        {
            // If not time freeze, detect illegal movement
            if (!tf.hold)
            {
                CameraMove();

                // Heart beat
                sound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
                if (illegal && !soundOn)
                {
                    // If illegal and haven't play sound, play heart beat
                    if (!sound[2].isPlaying)
                    {
                        sound[2].Play();
                    }
                    else
                    {
                        sound[2].UnPause();
                    }
                    soundOn = true;
                }
                if (!illegal)
                {
                    // If legal, pause heart beat
                    sound[2].Pause();
                    soundOn = false;
                }
            }

        }
    }
}
