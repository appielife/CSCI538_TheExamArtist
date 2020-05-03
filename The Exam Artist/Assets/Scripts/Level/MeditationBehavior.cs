using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/******************************************************************* 
Script for Meditation Skill:
Meditate for a few seconds. Cost one minute of test time.
A pair to MeditationHandler.cs
*******************************************************************/

public class MeditationBehavior : MonoBehaviour
{
    [Tooltip("Image for CD")]
    public Image imgCoolDown;
    [Tooltip("Text for CD")]
    public Text textCoolDown;
    [Tooltip("Audios for answers")]
    public AudioClip[] meditationAudioClips = new AudioClip[4];
    [Tooltip("Meditation Object")]
    public GameObject wall;
    [Tooltip("Level Object (Contains Classroom)")]
    public GameObject level;

    [HideInInspector]
    public int correctAns;

    private GameObject timer, testPaper;    // Select Handler and Timer Object
    private GameObject projectile;          // Projectile object

    // Cooldown settings, Fade setting
    private float coolDown = 15.0f, coolDownCounter = 15.0f, duration = 2.0f;
    private bool used = false, enoughTime = true;

    // Skill scripts
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
        testPaper = GameObject.FindGameObjectWithTag("MainSelectHandler");
        
        imgCoolDown.fillAmount = 0.0f;
        textCoolDown.text = "";
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    void Update()
    {
        // If not time freeze
        if (!tf.hold)
        {
            // If not enough time, cannot use skill anymore.
            if (timer.GetComponent<Timer>().timeLeft < 60 && enoughTime)
            {
                imgCoolDown.fillAmount = 1.0f;
                imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
                textCoolDown.text = "";
                enoughTime = false;
            }
            else if (enoughTime)
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
                }
            }
        }
    }

    // Function for skill (Main Function)
    public void Meditation()
    {
        if (!used && enoughTime)
        {
            timer.GetComponent<Timer>().timeLeft -= (60 - duration);
            used = true;

            // Reduce other skills' cooldown
            GameObject skills = GameObject.Find("SkillsScript");
            GodOfWashroomBehavior gow = skills.GetComponent<GodOfWashroomBehavior>();
            MagicCheatSheetBehavior mcs = skills.GetComponent<MagicCheatSheetBehavior>();
            GiftBlindEyesBehavior gbe = skills.GetComponent<GiftBlindEyesBehavior>();

            setting.setQuestion();

            if (gow.isTrigger() == true)
            {
                gow.ReduceCoolDownCounter(60);
            }

            if (mcs.isTrigger())
            {
                mcs.ResetSkill();
            }

            if (gbe.isCoolDown())
            {
                gbe.ReduceCoolDownCounter(60);
            }

            // Get correct answer of current question
            correctAns = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();

            // Set projectile to prevent falling
            projectile = setting.projectile;

            // Fade and switch object
            if (SteamVR.active)
            {
                FadeOut();
                Invoke("FadeIn", duration);
            }
            else
            {
                Initiate.Fade("", Color.black, 0.5f);
                Invoke("Change", duration);     // after 2 seconds, change object
            }
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
        level.SetActive(false);
        wall.SetActive(true);
        projectile.SetActive(false);
        SteamVR_Fade.Start(Color.clear, duration);
    }
    
    // Function to know if skill activated
    public bool isTrigger()
    {
        return used;
    }

    // Function to swap active object
    private void Change()
    {
        level.SetActive(false);
        wall.SetActive(true);
        projectile.SetActive(false);
    }
}
