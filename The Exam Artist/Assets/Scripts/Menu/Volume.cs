using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider slider;
    public Text volumeText;
    public float volume;
    private Settings setting;

    void Start()
    {
        slider.value = volume / 100.0f;
        volumeText.text = ((int)volume).ToString();
        setting = GameObject.Find("Settings").GetComponent<Settings>();
    }

    public void ToneUp()
    {
        if (volume < 100)
        {
            volume++;
            //slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
        }
    }

    public void ToneDown()
    {
        if (volume > 0)
        {
            volume--;
            //slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
            setting.setVolume((int)volume);
        }
    }
}
