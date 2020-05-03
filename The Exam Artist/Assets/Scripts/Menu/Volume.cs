using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/******************************************
Script to control set voulme in Menu scene
******************************************/

public class Volume : MonoBehaviour
{
    public Text volumeText;
    public float volume;
    private Settings setting;
    private AudioSource sound;
    private float initial = 0.5f;

    void Start()
    {
        volumeText.text = ((int)volume).ToString();
        setting = GameObject.Find("Settings").GetComponent<Settings>();
        sound = GameObject.Find("Sound").GetComponent<AudioSource>();
    }

    // Function for ToneUp button
    public void ToneUp()
    {
        if (volume < 100)
        {
            volume++;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
            sound.volume = initial * ((float)(setting.getVolume() - 50) / (float)50 + 1);
        }
    }
    
    // Function for ToneDown button
    public void ToneDown()
    {
        if (volume > 0)
        {
            volume--;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
            sound.volume = initial * ((float)(setting.getVolume() - 50) / (float)50 + 1);
        }
    }
}
