using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.Extras;
public class Navigator : MonoBehaviour
{
    private SteamVR_LaserPointer laserPointerL;
    private SteamVR_LaserPointer laserPointerR;
    private Settings hand;

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

        hand = (GameObject.Find("Settings")) ? GameObject.Find("Settings").GetComponent<Settings>() : null;

    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            switch (e.target.name)
            {
                case "Play":
                    GameObject blackboard = GameObject.Find("BlackBoard");
                    blackboard.transform.Find("MainMenu").gameObject.SetActive(false);
                    blackboard.transform.Find("HandSelect").gameObject.SetActive(true);
                    break;
                case "Left":
                    if (hand != null) { hand.setHand("LeftHand"); }
                    FadeIn();
                    Invoke("FadeOut", 5.0f);
                    break;
                case "Right":
                    if (hand != null) { hand.setHand("RightHand"); }
                    FadeIn();
                    Invoke("FadeOut", 5.0f);
                    break;
                case "TryAgain":
                    SceneManager.LoadScene(1);
                    break;
                default:
                    break;
            }
        }
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 1.0f);
            b.colors = cb;
        }
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            ColorBlock cb = b.colors;
            cb.normalColor = new Color(0.13f, 0.22f, 0.2f, 0.0f);
            b.colors = cb;
        }
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0.0f);
        SteamVR_Fade.Start(Color.black, 2.0f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0.0f);
        SteamVR_Fade.Start(Color.clear, 2.0f);
        SceneManager.LoadScene(1);
    }
}
