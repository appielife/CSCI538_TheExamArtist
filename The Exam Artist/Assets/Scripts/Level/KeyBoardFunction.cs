using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardFunction : MonoBehaviour
{
    private HideAndShowSkills hns;
    private GameObject cheatsheet;
    private float offset;
    private Transform originalPlace;

    private void Start()
    {
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;
        cheatsheet = GameObject.Find("CheatSheet");
        originalPlace = cheatsheet.transform;
    }
    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hns.Show();
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                hns.Hide();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log(originalPlace.position);
            cheatsheet.transform.position = new Vector3(originalPlace.position.x, originalPlace.position.y, 0.75f);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log(originalPlace.position);
            cheatsheet.transform.position = new Vector3(originalPlace.position.x, originalPlace.position.y, 1.0f);
        }
    }
}
