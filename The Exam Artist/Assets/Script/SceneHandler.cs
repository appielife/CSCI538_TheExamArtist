using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{

    public SteamVR_LaserPointer laserPointer;

    private string clicked = "";

    void Awake()
    {
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
        if(e.target.name == "TestPaper")
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
