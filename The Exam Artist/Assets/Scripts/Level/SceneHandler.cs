using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

/**************************************** 
Script to handle laser behavior in level
****************************************/

public class SceneHandler : MonoBehaviour
{
    [Tooltip("Skills canvas")]
    public GameObject SkillsOverlay;

    private Settings setting;
    private SteamVR_LaserPointer laserPointer;
    private AudioSource scribble;

    void Start()
    {
        // Set hand
        setting = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        string hand = (setting != null) ? setting.getHand() : "LeftHand";

        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject Hand = SteamVRObjects.transform.Find(hand).gameObject;

        // Activate pencil
        Hand.transform.Find("Pencil").gameObject.SetActive(true);

        // Set laser pointer actions
        laserPointer = Hand.GetComponent<SteamVR_LaserPointer>();
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        // Set table objects
        if (hand == "LeftHand")
        {
            GameObject.Find("PlayerTable").transform.Find("BottleLeft").gameObject.SetActive(true);
            GameObject.Find("PlayerTable").transform.Find("BottleRight").gameObject.SetActive(false);
            GameObject.Find("Projectile").transform.Find("EraserLeft").gameObject.SetActive(false);
            GameObject.Find("Projectile").transform.Find("EraserRight").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("PlayerTable").transform.Find("BottleLeft").gameObject.SetActive(false);
            GameObject.Find("PlayerTable").transform.Find("BottleRight").gameObject.SetActive(true);
            GameObject.Find("Projectile").transform.Find("EraserLeft").gameObject.SetActive(true);
            GameObject.Find("Projectile").transform.Find("EraserRight").gameObject.SetActive(false);
            Vector3 position = SkillsOverlay.transform.localPosition;
            position.z += 1.526f;
            SkillsOverlay.transform.localPosition = position;
        }

        // Set audio
        scribble = GameObject.FindGameObjectWithTag("TestSound").GetComponent<AudioSource>();
    }

    // Called when pointer clicks
    public void PointerClick(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            // If not selected choice
            if(b.tag != "MainChoiceSelected")
            {
                if (b.tag == "MainChoice")
                {
                    // If choice button
                    scribble.Play();
                }
                else
                {
                    // If other button
                    if (setting != null)
                    {
                        setting.click.Play();
                    }
                }
                // Invoke onclick if button enabled
                if (b.enabled)
                {
                    b.onClick.Invoke();
                }
            }
        }

    }

    // Called when pointer inside
    public void PointerInside(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            laserPointer.thickness = 0.002f; // Show laser
            Button b = e.target.gameObject.GetComponent<Button>();
            // If not selected choice, change color
            if (b.tag != "MainChoiceSelected")
            {
                ColorBlock cb = b.colors;
                cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                b.colors = cb;
            }
        }
        // If test paper (NOTE: BUGGY)
        if (e.target.tag == "MainTestPaper")
        {
            laserPointer.thickness = 0.002f; // Show laser
        }
    }

    // Called when pointer outside
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            laserPointer.thickness = 0.0f; // Hide laser
            Button b = e.target.gameObject.GetComponent<Button>();
            // If not selected choice, change color
            if (b.tag != "MainChoiceSelected")
            {
                ColorBlock cb = b.colors;
                cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                b.colors = cb;
            }
        }
        // If test paper (NOTE: BUGGY)
        if (e.target.tag == "MainTestPaper")
        {
            laserPointer.thickness = 0.0f; // Hide laser
        }
    }
}
