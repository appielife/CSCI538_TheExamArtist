using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************ 
Script to hide and show skill canvas
************************************/

public class HideAndShowSkills : MonoBehaviour
{
    private GameObject skillCanvas;

    void Awake()
    {
        GameObject table = GameObject.Find("PlayerTable");
        skillCanvas = table.transform.Find("SkillsOverlay").gameObject;
    }

    // Function to hide canvas
    public void Hide()
    {
        skillCanvas.SetActive(false);
    }

    // Function to show canvas
    public void Show()
    {
        skillCanvas.SetActive(true);
    }
}
