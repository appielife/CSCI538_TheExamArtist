using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***************************************************************
DEPRECATED 
Script for Spiritual Realm Skill.
When pressed, skill canvas changes to three options: 
one for freeze time, another for meditation, and last for return
***************************************************************/

public class SpiritualRealmBehavior : MonoBehaviour
{
    [Tooltip("Skills canvas")]
    public GameObject skills;

    public void SpiritualRealm()
    {
        for (int i = 0; i < 7; i++)
        {
            skills.transform.GetChild(i).gameObject.SetActive(i > 3);
        }
    }

    public void Back()
    {
        for (int i = 0; i < 7; i++)
        {
            skills.transform.GetChild(i).gameObject.SetActive(i < 4);
        }
    }
}
