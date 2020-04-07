﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/* 
   Bribe one of your classmate before the test.
   When you use the skill, you will use some special signal to call your partner,
   and your partner will call the teacher for "help" to attract teacher's attention.
   It will give a relatively safe period for you to cheat.
*/
public class GiftBlindEyesBehavior : MonoBehaviour
{
    public Image imgCoolDown, imgExist;
    public Text textCoolDown;

    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 15.0f, existTimeCounter = 15.0f;
    private bool exist = false, used = false;

    private AudioSource[] sound;
    public AudioClip[] giftAudioClip = new AudioClip[3];
    static System.Random songPlayer = new System.Random();

    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
    }


    void Update()
    {
        if (existTimeCounter > 0 && exist == true && used == false)
        {
            existTimeCounter -= Time.deltaTime;
            imgExist.fillAmount = 1 - existTimeCounter / existTime;
            textCoolDown.text = ((int)Mathf.Ceil(existTimeCounter)).ToString();
        }
        else if (existTimeCounter <= 0 && exist == true && used == false)
        {
            existTimeCounter = existTime;
            exist = false;
            imgExist.fillAmount = 0.0f;
            used = true;
        }
        else if (coolDownCounter > 0 && used == true)
        {
            coolDownCounter -= Time.deltaTime;
            imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
            textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
            //Debug.Log(coolDownCounter[0]);
        }
        else if (coolDownCounter <= 0 && used == true)
        {
            coolDownCounter = coolDown;
            textCoolDown.text = "";
            imgCoolDown.fillAmount = 0.0f;
            used = false;
        }
    }

    public bool isTrigger()
    {
        return exist == true;
    }

    public void GiftBlindEyes()
    {
        if (exist == false && used == false)
        {
            exist = true;
            int n = songPlayer.Next(3);
            sound[0].PlayOneShot(giftAudioClip[n], 1.5f);
        }
    }
}
