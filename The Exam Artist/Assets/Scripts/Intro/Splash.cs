using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;

/***********************************
Script for Intro scene.
Used for USC then logo.
***********************************/

public class Splash : MonoBehaviour
{
    [Tooltip("Logo Image")]
    public Image logo;
    [Tooltip("USC Image")]
    public Image usc;
    [Tooltip("Load Scene Handler (SteamVR Load Level)")]
    public GameObject LoadSceneHandler;
    /*[Tooltip("Video Projector")]
    public GameObject videoplayer;
    [Tooltip("Video Player")]
    private VideoPlayer video;*/
    private bool usc_in = false, usc_out = false, logo_in = false, logo_out = false;
    private bool usc_out_s = false, logo_in_s = false, logo_out_s = false;
    private Light[] lights;

    void Start()
    {
        /*video = videoplayer.GetComponent<VideoPlayer>();
        video.loopPointReached += EndReached;
        video.Play();*/
        if (SteamVR.active)
        {
            // If VR
            StartCoroutine(FadeIn(usc, 4.0f));
        }
        else
        {
            // If no VR
            lights = FindObjectsOfType(typeof(Light)) as Light[];
            foreach (Light light in lights)
            {
                light.intensity = 0;
            }
            Initiate.Fade("Menu", Color.black, 4.0f);
        }
    }

    void Update()
    {
        /*if (!videoplayer.activeSelf && !logo_in_s)
        {
            StartCoroutine(FadeIn(logo, 2.0f));
            logo_in_s = true;
        }*/
        if (!usc_out && usc_in && !usc_out_s)
        {
            StartCoroutine(FadeOut(usc, 1.0f));
            usc_out_s = true;
        }
        else if (!logo_in && usc_out && !logo_in_s)
        {
            StartCoroutine(FadeIn(logo, 4.0f));
            logo_in_s = true;
        }
        else if (!logo_out && logo_in && !logo_out_s)
        {
            StartCoroutine(FadeOut(logo, 1.0f));
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
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.3f);
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
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.3f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == usc) { usc_in = true; }
        if (i == logo) { logo_in = true; }
    }

    /*void EndReached(VideoPlayer vp)
    {
        videoplayer.SetActive(false);
    }*/
}
