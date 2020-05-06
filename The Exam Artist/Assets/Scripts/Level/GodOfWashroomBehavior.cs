using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;

/******************************************************************* 
Script for Washroom Skill:
Go to the washroom for a few seconds. Cost one minute of test time.
A pair to washroom.cs
*******************************************************************/

public class GodOfWashroomBehavior : MonoBehaviour
{
    [Tooltip("Image for CD")]
    public Image imgCoolDown;
    [Tooltip("Text for CD")]
    public Text textCoolDown;
    [Tooltip("Text for limitation")]
    public Text limitText;
    [Tooltip("Audios for skill")]
    public AudioClip[] washroomAudioClips = new AudioClip[7];
    [Tooltip("Washroom Object")]
    public GameObject Washroom;
    [Tooltip("Level Object (Contains Classroom)")]
    public GameObject Level;

    // Cooldown settings, Fade setting
    private float coolDown = 14.0f, coolDownCounter = 14.0f, duration = 2.0f;
    private bool used = false, enoughTime = true;
    private int limit = 5;                  // Number of time to use skill

    private GameObject testPaper, timer;    // Select Handler and Timer Object
    private GameObject projectile;          // Projectile object
    private GameObject cheatsheet;          // Cheat sheet object
    private AudioSource[] sound;            // Audio for behavior

    // Skill scripts
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
        testPaper = GameObject.FindGameObjectWithTag("MainSelectHandler");
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        imgCoolDown.fillAmount = 0.0f;
        textCoolDown.text = "";
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        limitText.text = limit.ToString();
        cheatsheet = GameObject.FindGameObjectWithTag("CheatSheet");
    }

    void Update()
    {
        // If not time freeze
        if (!tf.hold)
        {
            // If not enough time or limit reached, cannot use skill anymore.
            if (timer.GetComponent<Timer>().timeLeft < 60 && enoughTime || limit == 0)
            {
                imgCoolDown.fillAmount = 1.0f;
                imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
                textCoolDown.text = "";
                enoughTime = false;
            }
            else
            {
                // Cooldown remains active
                if (coolDownCounter > 0 && used == true)
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
                    limitText.text = limit.ToString();
                    if (limit == 0)
                    {
                        limitText.text = "";
                    }
                }
            }
        }
    }

    // Function for skill (Main Function)
    public void GodOfWashroom()
    {
        if (used == false && limit > 0)
        {
            if (timer.GetComponent<Timer>().timeLeft < 60)
            {
                // Not enough time to go to washroom
                sound[0].PlayOneShot(washroomAudioClips[6], 1.0f);
            }
            else
            {
                // Reduce time
                limitText.text = "";
                limit--;
                timer.GetComponent<Timer>().timeLeft -= (60 - duration);
                used = true;

                // Set settings to pass to washroom
                setting.setQuestion();
                setting.setTime();
                // Set projectile to prevent falling
                projectile = setting.projectile;

                // Fade and switch object
                if (SteamVR.active)
                {
                    FadeOut();                      // 2 seconds to fade out
                    Invoke("Change", duration);     // after 2 seconds, change object
                    Invoke("FadeIn", duration * 2); // after 4 seconds, fade in for 2 seconds (NOTE: BUGGY)
                }
                else
                {
                    Initiate.Fade("", Color.black, 0.5f);
                    Invoke("Change", duration);     // after 2 seconds, change object
                }
            }
        }
        else
        {
            if (limit == 0)
            {
                // Limit reached
                sound[0].PlayOneShot(washroomAudioClips[5], 1.0f);
            }
            else
            {
                // Skill is cooling down
                sound[0].PlayOneShot(washroomAudioClips[4], 1.0f);
            }
        }
    }

    // Function to know if skill activated
    public bool isTrigger()
    {
        return used;
    }

    // Function to get cooldown counter
    public float GetCoolDownCounter()
    {
        return coolDownCounter;
    }

    // Function to reduce cooldown
    public void ReduceCoolDownCounter(float n)
    {
        if (n == -1)
        {
            coolDownCounter = coolDown;
        }
        else
        {
            coolDownCounter -= n;
        }
    }

    // Function to fade out (SteamVR)
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    // Function to fade in (SteamVR)
    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, duration);
    }

    // Function to swap active object
    private void Change()
    {
        Washroom.SetActive(true);
        Level.SetActive(false);
        projectile.SetActive(false);
        cheatsheet.SetActive(false);
    }
}
