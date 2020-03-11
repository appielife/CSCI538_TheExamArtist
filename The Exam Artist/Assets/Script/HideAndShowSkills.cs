using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowSkills : MonoBehaviour
{
    public GameObject skillCanvas;

    public void Hide()
    {
        skillCanvas.SetActive(false);
    }

    public void Show()
    {
        skillCanvas.SetActive(true);
    }
}
