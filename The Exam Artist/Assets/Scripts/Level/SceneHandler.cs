using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{

<<<<<<< HEAD:The Exam Artist/Assets/Script/SceneHandler.cs
    public SteamVR_LaserPointer laserPointer;

    private string clicked = "";
    
    void Awake()
=======
    private SteamVR_LaserPointer laserPointer;

    private string clicked = "";
    
    void Start()
>>>>>>> 3e4a7def4b33fe39b56bc27272105f6517cd6559:The Exam Artist/Assets/Scripts/Level/SceneHandler.cs
    {
        Settings setting = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        string hand = (setting != null) ? setting.getHand() : "LeftHand";

        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject Hand = SteamVRObjects.transform.Find(hand).gameObject;

        Hand.transform.Find("Pencil").gameObject.SetActive(true);

        laserPointer = Hand.GetComponent<SteamVR_LaserPointer>();

        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if(e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            b.onClick.Invoke();
            if (e.target.name != "Next" && e.target.name != "Prev")
            {
                clicked = e.target.name;
            }
        }
  
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null && clicked != e.target.name)
        {
            laserPointer.thickness = 0.002f;
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            b.colors = cb;
        }
        if(e.target.name == "TestPaper" )
        {
            laserPointer.thickness = 0.002f;
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null && clicked != e.target.name)
        {
            laserPointer.thickness = 0.0f;
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            b.colors = cb;
        }
        if (e.target.name == "TestPaper")
        {
            laserPointer.thickness = 0.0f;
        }
    }
}
