using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider slider;
    public Text volumeText;
    public float volume;

    void Start()
    {
        slider.value = volume / 100.0f;
        volumeText.text = ((int)volume).ToString();
    }

    public void ToneUp()
    {
        if (volume < 100)
        {
            volume++;
            slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
        }
    }

    public void ToneDown()
    {
        if (volume > 0)
        {
            volume--;
            slider.value = volume / 100.0f;
            volumeText.text = ((int)volume).ToString();
        }
    }
}
