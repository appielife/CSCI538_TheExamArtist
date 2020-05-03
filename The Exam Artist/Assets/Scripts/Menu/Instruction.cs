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

    private Text main;
    private string init = "Become the one and only EXAM ARTIST!";

    void Start()
    {
        main = GameObject.Find("InstructionText").GetComponent<Text>();
        main.text = init;
    }

    public void showInstructionX()
    {
        main.text = "Go to washroom for 10 seconds and at most 5 times\n (cost 1 min)";
    }

    public void showInstructionY()
    {
        main.text = "Show hint on your magic cheat sheet for 5 seconds";
    }

    public void showInstructionA()
    {
        main.text = "Meditate to think about the current answer \n(cost 1 min)";
    }

    public void showInstructionB()
    {
        main.text = "Let your classmate(s) distract the teacher";
    }

    public void showInstructionG()
    {
        main.text = "Hold to stop time for a total of 10 seconds";
    }
}
