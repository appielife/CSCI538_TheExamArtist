﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllegalMoveHandler : MonoBehaviour
{
    public GameObject playerCam;
    private AudioSource[] sound;
    public bool illegal = false;
    private bool soundOn = false;
    public Text debugText;
    private float timeLeft = 0.0f;
    private LevelSetting setting;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        playerCam = SteamVRObjects.transform.Find("VRCamera").gameObject;
        debugText = playerCam.transform.Find("Debug").gameObject.GetComponentInChildren<Text>();
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.washroomed)
        {
            timeLeft = 5.0f;
        }
    }

    void CameraMove()
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
        if (!setting.onPrepare && setting.illegalDetect)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                CameraMove();
                sound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
                if (illegal && !soundOn)
                {
                    if (!sound[2].isPlaying)
                    {
                        sound[2].Play();
                    }
                    else
                    {
                        sound[2].UnPause();
                    }
                    //debugText.text = "Illegal";
                    soundOn = true;
                }
                if (!illegal)
                {
                    sound[2].Pause();
                    //debugText.text = "Legal";
                    soundOn = false;
                }
            }
        }
    }
}
