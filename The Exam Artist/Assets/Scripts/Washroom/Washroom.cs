using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

/********************************
Script for behavior in washroom
A pair to GodOfWashroom.cs
********************************/

public class Washroom : MonoBehaviour
{
    [Tooltip("Washroom Object")]
    public GameObject current;
    [Tooltip("Level Object (Contains Classroom)")]
    public GameObject Level;

    private GameObject projectile;      // Projectile object
    private GameObject cheatsheet;      // Cheat sheet object
    private LevelSetting setting;       // Global setting
    private float timer;                // Time for staying in washroom
    private GameObject paper;           // Answer paper
    private float duration = 2.0f;      // Fade setting
    private bool isWashroom = false;    // Is in washroom

    void OnEnable()
    {
        isWashroom = true;
        List<int> randomArray = new List<int>();
        if (GameObject.Find("LevelSetting") != null)
        {
            setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
            timer = setting.washroomDuration;
            projectile = setting.projectile;
            cheatsheet = setting.cheatsheet;

            paper = GameObject.Find("Paper");

            int time = (int)Mathf.Ceil((float)setting.timeLeft / 60.0f);
            string text = "";

            // Set propability of number of answers to get
            if (setting.timeLeft > (float)setting.initialTime / 2.0f)
            {
                // If remaing time > half test time, 0 or 1 answer
                for (int i = 0; i < time; i++)
                {
                    randomArray.Add(0);
                }
                randomArray.Add(1);
            }
            else
            {
                // If remaing time < half test time, 1 or 2 answer(s)
                for (int i = 0; i < time + (int)Mathf.Ceil((float)setting.initialTime / 120.0f); i++)
                {
                    randomArray.Add(1);
                }
                randomArray.Add(2);
            }

            // Pick a number
            int numQues = randomArray[Random.Range(0, randomArray.Count)];
            // Set unanswered question(s) counter
            List<int> counter = new List<int>();
            for (int i = 0; i < setting.unansweredQues.Count; i++)
            {
                counter.Add(i);
            }
            // Show result
            for (int i = 0; i < numQues; i++)
            {
                int tempIdx = Random.Range(0, counter.Count);
                int tempQues = counter[tempIdx];
                counter.RemoveAt(tempIdx);
                text += setting.answer[setting.unansweredQues[tempQues]] + "\n";
            }
            if (numQues == 0)
            {
                text = "Unfortunately, I failed to find answers :(\n\nBetter luck next time~";
            }
            paper.GetComponentInChildren<Text>().text = text;
        }
    }

    void Update()
    {
        if (GameObject.Find("LevelSetting") != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if (isWashroom)
                {
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
                    isWashroom = false;
                }
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
        SteamVR_Fade.Start(Color.clear, duration);
    }


    private void Change()
    {
        Level.SetActive(true);
        current.SetActive(false);
        projectile.SetActive(true);
        cheatsheet.SetActive(true);
    }

    public bool inWashroom()
    {
        return isWashroom;
    }
}