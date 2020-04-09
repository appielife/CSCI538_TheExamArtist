using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardFunction : MonoBehaviour
{
    private HideAndShowSkills hns;
    private float offset;
    private Transform originalPlace;

    private void Start()
    {
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;
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
    }
}
