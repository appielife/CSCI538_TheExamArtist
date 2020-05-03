using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/********************************************************************************** 
Script for Bribe Skill:
Bribe one of your classmate before the test.
When you use the skill, you will use some special signal to call your partner,
and your partner will call the teacher for "help" to attract teacher's attention.
It will give a relatively safe period for you to cheat.
***********************************************************************************/

public class GiftBlindEyesBehavior : MonoBehaviour
{
    [Tooltip("Image for CD")]
    public Image imgCoolDown;
    [Tooltip("Image for existing")]
    public Image imgExist;
    [Tooltip("Text for CD")]
    public Text textCoolDown;
    [Tooltip("Audios for calling teacher")]
    public AudioClip[] giftAudioClip = new AudioClip[3];

    [HideInInspector]
    public List<Sprite> bribeList = new List<Sprite>();
    [HideInInspector]
    public string target;
    [HideInInspector]
    public string tempChoice = "";
    [HideInInspector]
    public Sprite[] buttons;

    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 15.0f, existTimeCounter = 15.0f;
    private bool exist = false, used = false;
    private AudioSource[] sound;
    private TimeFreezeBehavior tf;
    static System.Random songPlayer = new System.Random();

    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();

        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        bribeList = setting.bribeList; // Link Reference

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }


    void Update()
    {
        // If not time freeze, cooldown remains active
        if (!tf.hold)
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
            }
            else if (coolDownCounter <= 0 && used == true)
            {
                coolDownCounter = coolDown;
                textCoolDown.text = "";
                imgCoolDown.fillAmount = 0.0f;
                used = false;
            }
        }
    }

    // Function to know if skill activated
    public bool isTrigger()
    {
        return exist == true;
    }

    // Function to know if skill is cooling down
    public bool isCoolDown()
    {
        return used == true || exist == true;
    }

    // Function to reduce cooldown
    public void ReduceCoolDownCounter(float n)
    {
        // If is cooling down
        if (exist)
        {
            // Clamp n to counter range
            if (n > existTimeCounter)
            {
                n -= existTimeCounter;
                existTimeCounter = existTime;
                exist = false;
            }
            else
            {
                existTimeCounter -= n;
                n = 0;
            }
        }
        if (n > 0)
        {
            imgExist.fillAmount = 0.0f;
            used = true;
            coolDownCounter -= n;
        }
    }

    // Function to choose which student
    public void ChooseBribee(GameObject t)
    {
        target = t.GetComponent<Image>().sprite.name;
        GiftBlindEyes();
    }

    // Function for skill (Main Function ChooseBribee in TestPaperBehavior.cs)
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
