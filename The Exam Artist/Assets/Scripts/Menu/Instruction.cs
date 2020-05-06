using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using UnityEngine.UI;

/********************************************
Script for editing instructions in menu scene  
********************************************/

public class Instruction : MonoBehaviour
{
    [Tooltip("Skill Object (Contains Image)")]
    public GameObject skill;
    [Tooltip("Logo Object (Contains Image)")]
    public GameObject logo;
    [Tooltip("Skill Image Sprites")]
    public Sprite[] images;

    private Text main;
    private string init = "Become the one and only EXAM ARTIST!";
    private Image skillImage;

    void Start()
    {
        main = GameObject.Find("InstructionText").GetComponent<Text>();
        main.text = init;
        skillImage = skill.GetComponent<Image>();
    }

    public void showInstructionX()
    {
        main.text = "Go to washroom for 10 seconds and at most 5 times\n (cost 1 min)";
        logo.SetActive(false);
        skill.SetActive(true);
        skillImage.sprite = images[0];
    }

    public void showInstructionY()
    {
        main.text = "Show hint on your magic cheat sheet for 5 seconds";
        logo.SetActive(false);
        skill.SetActive(true);
        skillImage.sprite = images[1];
    }

    public void showInstructionA()
    {
        main.text = "Meditate to think about the current answer \n(cost 1 min)";
        logo.SetActive(false);
        skill.SetActive(true);
        skillImage.sprite = images[2];
    }

    public void showInstructionB()
    {
        main.text = "Let your classmate(s) distract the teacher";
        logo.SetActive(false);
        skill.SetActive(true);
        skillImage.sprite = images[3];
    }

    public void showInstructionG()
    {
        main.text = "Hold to stop time for a total of 10 seconds";
        logo.SetActive(false);
        skill.SetActive(true);
        skillImage.sprite = images[4];
    }
}
