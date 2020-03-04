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
    }
    public void PointerInside(object sender, PointerEventArgs e)
    {
    }
    public void PointerOutside(object sender, PointerEventArgs e)
    {
    }
}
