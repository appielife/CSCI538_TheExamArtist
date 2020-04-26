using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public Image usc, logo;
    public GameObject LoadSceneHandler;
    private bool usc_out = false, logo_in = false, logo_out = false;
    private bool logo_in_s = false, logo_out_s = false;

    void Start()
    {
        StartCoroutine(FadeOut(usc, 2.0f));
    }

    void Update()
    {
        if (!logo_in && usc_out && !logo_in_s)
        {
            StartCoroutine(FadeIn(logo, 2.0f));
            logo_in_s = true;
        }
        else if (!logo_out && logo_in && !logo_out_s)
        {
            StartCoroutine(FadeOut(logo, 2.0f));
            logo_out_s = true;
        }
        else if (logo_out)
        {
            LoadSceneHandler.SetActive(true);
        }
    }

    private IEnumerator FadeOut(Image i, float FadeTime)
    {
        while (i.color.a > 0)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == usc) { usc_out = true; }
        if (i == logo) { logo_out = true; }
    }

    private IEnumerator FadeIn(Image i, float FadeTime)
    {
        while (i.color.a < 1)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == logo) { logo_in = true; }
    }

}
