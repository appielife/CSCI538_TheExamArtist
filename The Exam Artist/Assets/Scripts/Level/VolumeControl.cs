using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
Script to adjust volume using global setting (if exist)
*******************************************************/

public class VolumeControl : MonoBehaviour
{
    private Settings setting;
    private AudioSource[] sound;

    void Start()
    {
        sound = GetComponents<AudioSource>();
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
            for (int i = 0; i < sound.Length; i++)
            {
                sound[i].volume = sound[i].volume * ((float)(setting.getVolume() - 50) / (float)50 + 1);
            }
        }
    }
}
