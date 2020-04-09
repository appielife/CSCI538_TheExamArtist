using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using UnityEngine.UI;

public class Instruction : MonoBehaviour
{
    private Text main, shadow;
    private string init = "Try to CHEAT and don't get caught!";
    private SteamVR_LaserPointer laserPointerL;
    private SteamVR_LaserPointer laserPointerR;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject LeftHand = SteamVRObjects.transform.Find("LeftHand").gameObject;
        GameObject RightHand = SteamVRObjects.transform.Find("RightHand").gameObject;

        laserPointerL = LeftHand.GetComponent<SteamVR_LaserPointer>();
        laserPointerR = RightHand.GetComponent<SteamVR_LaserPointer>();

        laserPointerL.thickness = 0.002f;
        laserPointerR.thickness = 0.002f;

        laserPointerL.PointerIn += PointerInside;
        laserPointerL.PointerOut += PointerOutside;
        laserPointerL.PointerClick += PointerClick;
        laserPointerR.PointerIn += PointerInside;
        laserPointerR.PointerOut += PointerOutside;
        laserPointerR.PointerClick += PointerClick;

        main = GameObject.Find("InstructionText").GetComponent<Text>();
        shadow = GameObject.Find("InstructionText2").GetComponent<Text>();

        main.text = init;
        shadow.text = init;
    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        switch (e.target.name)
        {
            case "X Button":
                showInstructionX();
                break;
            case "Y Button":
                showInstructionY();
                break;
            case "A Button":
                showInstructionA();
                break;
            case "B Button":
                showInstructionB();
                break;
        }
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
    }

    public void showInstructionX()
    {
        main.text = "Show hint on your magic cheat sheet \n(5 sec)";
        shadow.text = main.text;
    }

    public void showInstructionY()
    {
        main.text = "Go to washroom for 10 seconds \n(-2 min)";
        shadow.text = main.text;
    }

    public void showInstructionA()
    {
        main.text = "Let your buddy to distract teacher";
        shadow.text = main.text;
    }

    public void showInstructionB()
    {
        main.text = "Meditate to think about answer \n(-1 min)";
        shadow.text = main.text;
    }
}
