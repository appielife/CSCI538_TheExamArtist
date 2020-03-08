using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
using UnityEngine.SceneManagement;
public class Navigator : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointerL; 
    public SteamVR_LaserPointer laserPointerR;

    void Awake()
    {
        laserPointerL.PointerIn += PointerInside;
        laserPointerL.PointerOut += PointerOutside;
        laserPointerL.PointerClick += PointerClick;
        laserPointerR.PointerIn += PointerInside;
        laserPointerR.PointerOut += PointerOutside;
        laserPointerR.PointerClick += PointerClick;
    }
    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.gameObject.GetComponent<Button>() != null)
        {
            Button b = e.target.gameObject.GetComponent<Button>();
            b.onClick.Invoke();

            if (e.target.name == "Play")
            {
                SceneManager.LoadScene(1);
            }

            if (e.target.name == "Left")
            {
                SceneManager.LoadScene(2);

            }

            if (e.target.name == "Right")
            {
                SceneManager.LoadScene(2);
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
}
