using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;

public class GodOfWashroomBehavior : MonoBehaviour
{
    public Image imgCoolDown;
    public Text textCoolDown;
    public GameObject timer;
    public GameObject testPaper;
    private AudioSource[] sound;
    public AudioClip[] washroomAudioClips = new AudioClip[7];
    private float coolDown = 10.0f;
    private float coolDownCounter = 10.0f;
    private bool used = false;
    private int limit = 5;
    private float duration = 2.0f;
    private LevelSetting setting;

    private void loadResources()
    {
        GameObject table = GameObject.Find("PlayerTable");
        GameObject SkillsOverlay = table.transform.Find("SkillsOverlay").gameObject;
        GameObject SkillCoolDown = SkillsOverlay.transform.Find("SkillCoolDown").gameObject;
        GameObject skill = SkillCoolDown.transform.Find("GodOfWashroom").gameObject;
        GameObject resources = skill.transform.Find("Image").gameObject;

        imgCoolDown = resources.transform.Find("CDImg").gameObject.GetComponent<Image>();
        textCoolDown = resources.transform.Find("CDText").gameObject.GetComponent<Text>();
    }

    void Start()
    {
        loadResources();
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        imgCoolDown.fillAmount = 0.0f;
        textCoolDown.text = "";
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.washroomed)
        {
            used = true;
        }
    }

    void Update()
    {
       if (coolDownCounter > 0 && used == true)
        {
            coolDownCounter -= Time.deltaTime;
            imgCoolDown.fillAmount = 1  - coolDownCounter / coolDown;
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

    public void GodOfWashroom()
    {
        if (used == false && limit > 0)
        {
            if (timer.GetComponent<Timer>().timeLeft < 150)
            {
                sound[0].PlayOneShot(washroomAudioClips[6], 1.0f);
                //Debug.Log("Not enough time to go to washroom!");
            }
            else
            {
                FadeIn();
                Invoke("FadeOut", duration);
                timer.GetComponent<Timer>().timeLeft -= (120 - duration);
                used = true;

                //int correctAns = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
                //sound[0].PlayOneShot(washroomAudioClips[correctAns], 1.0f);
                //Debug.Log("The correct answer is " + correctAns.ToString());
                limit -= 1;

                setting.timeLeft = timer.GetComponent<Timer>().timeLeft;
                setting.setWashroom();
                setting.setQuestion();
                SceneManager.LoadScene(3);
            }
        }
        else
        {
            if (limit == 0)
            {
                sound[0].PlayOneShot(washroomAudioClips[5], 1.0f);
                //Debug.Log("You can just use this skill twice per test");
            }
            else
            {
                sound[0].PlayOneShot(washroomAudioClips[4], 1.0f);
                //Debug.Log("The skill is cooling down.");
            }
        }
    }

    public bool isTrigger()
    {
        return used;
    }

    public float GetCoolDownCounter()
    {
        return coolDownCounter;
    }

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

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.Start(Color.black, duration);
    }

    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, duration);
    }
}
