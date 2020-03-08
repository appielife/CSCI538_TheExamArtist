using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
public class Navigator : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;
    public GameObject selectHandler;
    private string clicked = "";

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            if (e.target.name == "PLAY")
            {
                GameObject question = e.target.parent.parent.GetChild(0).gameObject;
                GameObject A = e.target.parent.parent.GetChild(1).GetChild(0).gameObject;
                GameObject B = e.target.parent.parent.GetChild(1).GetChild(1).gameObject;
                GameObject C = e.target.parent.parent.GetChild(1).GetChild(2).gameObject;
                GameObject D = e.target.parent.parent.GetChild(1).GetChild(3).gameObject;
                A.GetComponentInChildren<Text>().text = "blablablabla";

                //selectHandler.GetComponent<MultipleChoiceBehavior>().reset();
            }
            else
            {
                Button b = e.target.gameObject.GetComponent<Button>();
                b.onClick.Invoke();
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
        if (e.target.name == "TestPaper")
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
