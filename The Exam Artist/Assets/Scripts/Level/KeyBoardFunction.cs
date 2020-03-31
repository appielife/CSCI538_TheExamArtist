using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardFunction : MonoBehaviour
{
    private HideAndShowSkills hns;
    public float offset;

    private void Start()
    {
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
    }
    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                hns.Show();
            }
            else
            {
                hns.Hide();
            }
        }
    }
}
