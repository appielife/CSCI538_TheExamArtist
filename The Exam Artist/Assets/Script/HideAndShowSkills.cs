using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowSkills : MonoBehaviour
{
    public GameObject skillCanvas;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayer");
        GameObject SteamVRObjects = player.transform.Find("SteamVRObjects").gameObject;
        GameObject VRCamera = SteamVRObjects.transform.Find("VRCamera").gameObject;
        skillCanvas = VRCamera.transform.Find("SkillsOverlay").gameObject;
    }
    public void Hide()
    {
        skillCanvas.SetActive(false);
    }

    public void Show()
    {
        skillCanvas.SetActive(true);
    }
}
