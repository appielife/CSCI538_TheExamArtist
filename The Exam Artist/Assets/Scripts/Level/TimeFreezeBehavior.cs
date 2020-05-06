using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Valve.VR;

/******************************************************************
Script for Freeze Skill:
Hold to freeze time for a maximum of N seconde for the whole test.
******************************************************************/

public class TimeFreezeBehavior : MonoBehaviour
{
    [Tooltip("Image for CD")]
    public Image imgCoolDown;
    [Tooltip("Text for remaining seconds")]
    public Text limitNum;
    [Tooltip("Audio for skill")]
    public AudioClip timeFreezeAudioClip;

    [HideInInspector]
    public bool hold = false; // If holding freeze skill button/trigger

    // All characters
    private GameObject teacherCharacter, joggingCharacter, outsideCharacter, hallwayCharacter;
    private GameObject[] studentCharacters;

    private float teacherSpeed;     // Teacher's walking speed
    private float limit = 10.0f;    // Seconds remaining
    private bool exist = false, used = false, wasRunning = false;
    private AudioSource[] sound;    // Set sound


    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        limitNum.text = limit.ToString();

        // Set characters
        teacherCharacter = GameObject.FindGameObjectWithTag("TeacherAction");
        teacherSpeed = teacherCharacter.GetComponent<NavMeshAgent>().speed;
        studentCharacters = GameObject.FindGameObjectsWithTag("StudentCharacter");
        joggingCharacter = GameObject.FindGameObjectWithTag("JoggingCharacter");
        outsideCharacter = GameObject.FindGameObjectWithTag("OutsideCharacter");
        hallwayCharacter = GameObject.FindGameObjectWithTag("HallwayCharacter");

        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
        imgCoolDown.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        limit = setting.maxFreezeTime;
    }

    void Update()
    {
        // If holding and has remaining time, freeze
        if (hold && limit > 0)
        {
            TimeFreeze();
            limit -= Time.deltaTime;
            limitNum.text = ((int)limit).ToString();
            imgCoolDown.fillAmount = 1 - limit / 10.0f;
        }
        else if ((!hold && exist) || limit < 0)
        {
            // If no more time or not holding
            UnfreezeCharacters();
            exist = false;
            if (limit < 0)
            {
                imgCoolDown.fillAmount = 1.0f;
                limitNum.text = "";
            }
        }
    }

    // Function for skill (Main Function)
    public void TimeFreeze()
    {
        if (exist == false && used == false)
        {
            // Play freeze audio
            sound[0].PlayOneShot(timeFreezeAudioClip, 1.5f);

            // Freeze all characters
            teacherCharacter.GetComponent<Animator>().enabled = false;
            teacherCharacter.GetComponent<NavMeshAgent>().speed = 0;
            teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 0;
            for (int i = 0; i < studentCharacters.Length; i++)
            {
                studentCharacters[i].GetComponent<Animator>().enabled = false;
            }
            joggingCharacter.GetComponent<Animator>().enabled = false;
            hallwayCharacter.GetComponent<Animator>().enabled = false;
            hallwayCharacter.GetComponent<male1>().enabled = false;
            outsideCharacter.GetComponent<Animator>().enabled = false;
            if (outsideCharacter.GetComponent<femaleoutside>().enabled)
            {
                wasRunning = true;
                outsideCharacter.GetComponent<femaleoutside>().enabled = false;
            }
            exist = true;
        }
    }

    // Function for skill (Main Function)
    private void UnfreezeCharacters()
    {
        // Unfreeze all characters
        teacherCharacter.GetComponent<Animator>().enabled = true;
        teacherCharacter.GetComponent<NavMeshAgent>().speed = teacherSpeed;
        teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 120;
        for (int i = 0; i < studentCharacters.Length; i++)
        {
            studentCharacters[i].GetComponent<Animator>().enabled = true;
        }
        joggingCharacter.GetComponent<Animator>().enabled = true;
        hallwayCharacter.GetComponent<Animator>().enabled = true;
        hallwayCharacter.GetComponent<male1>().enabled = true;
        outsideCharacter.GetComponent<Animator>().enabled = true;
        if (wasRunning)
        {
            outsideCharacter.GetComponent<femaleoutside>().enabled = true;
        }
    }

    // Function to know if skill activated
    public bool isExisting()
    {
        return exist;
    }
}