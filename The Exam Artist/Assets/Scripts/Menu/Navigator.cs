using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;

/*******************************************************************
Script for handling LaserPointer actions in Menu and GameOver scene.
*******************************************************************/

public class Navigator : MonoBehaviour
{
    [Tooltip("Load Scene Handler (SteamVR Load Level)")]
    public GameObject LoadSceneHandler;

    private SteamVR_LaserPointer laserPointerL, laserPointerR;
    private Settings setting;
    private Volume volume;
    private LevelSetting levelsetting;
    private ScoreCalculate report;

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

        setting = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        if (GameObject.Find("VolumeHandler"))
        {
            volume = GameObject.Find("VolumeHandler").GetComponent<Volume>();
        }
        if (GameObject.Find("ScoreCalculate"))
        {
            report = GameObject.Find("ScoreCalculate").GetComponent<ScoreCalculate>();
        }
    }

    // Called when pointer clicks
    public void PointerClick(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            b.onClick.Invoke(); // Invoke button onclick event
            if (setting != null)
            {
                setting.click.Play(); // Play click sound
            }
        }
    }

    // Called when pointer inside
    public void PointerInside(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            // Change button color
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 1.0f);
            if (e.target.tag == "InstructionButton")
            {
                cb.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            b.colors = cb;
        }
    }

    // Called when pointer outside
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        // If button
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            // Change button color
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 0.0f);
            if (e.target.tag == "InstructionButton")
            {
                cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            b.colors = cb;
        }
    }

    // Onclick event for Play button
    public void Play()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("MainMenu").gameObject.SetActive(false);
        blackboard.transform.Find("HandSelect").gameObject.SetActive(true);
    }

    // Onclick event for Options button
    public void Options()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("MainMenu").gameObject.SetActive(false);
        blackboard.transform.Find("OptionMenu").gameObject.SetActive(true);
    }

    // Onclick event for Quit button
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Onclick event for Return button
    public void Return()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("OptionMenu").gameObject.SetActive(false);
        blackboard.transform.Find("MainMenu").gameObject.SetActive(true);
    }

    // Onclick event for Left button
    public void Left()
    {
        if (setting != null) {
            setting.setHand("LeftHand"); // Store hand setting to setting
        }

        // Load Scene
        if (SteamVR.active) 
        {
            // If VR
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            // If non-VR
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }

    // Onclick event for Right button
    public void Right()
    {
        if (setting != null) {
            setting.setHand("RightHand"); // Store hand setting to setting
        }

        // Load Scene
        if (SteamVR.active)
        {
            // If VR
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            // If non-VR
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }

    // Onclick event for Back button
    public void Back()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("HandSelect").gameObject.SetActive(false);
        blackboard.transform.Find("MainMenu").gameObject.SetActive(true);
    }

    // Onclick event for TryAgain button
    public void TryAgain()
    {
        if (setting != null)
        {
            setting.setFailed(false);
        }
        if (SteamVR.active)
        {
            // If VR
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            // If non-VR
            Initiate.Fade("Level 1", Color.black, 0.5f);
        }
    }
    
    // Onclick event for Continue button
    public void Continue()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(false);
        blackboard.transform.Find("DecideMenu").gameObject.SetActive(true);
    }

    // Onclick event for ShowReport button
    public void ShowReport()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreReport").gameObject.SetActive(true);
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(false);
        blackboard.transform.Find("Props").gameObject.SetActive(false);
    }

    // Onclick event for DecideBack button
    public void DecideBack()
    {
        GameObject blackboard = GameObject.Find("BlackBoard");
        blackboard.transform.Find("ScoreReport").gameObject.SetActive(false);
        blackboard.transform.Find("ScoreMenu").gameObject.SetActive(true);
        blackboard.transform.Find("Props").gameObject.SetActive(true);
    }

    // Onclick event for PrevReport button
    public void PrevReport()
    {
        report.prevReport();
    }

    // Onclick event for NextReport button
    public void NextReport()
    {
        report.nextReport();
    }
}
