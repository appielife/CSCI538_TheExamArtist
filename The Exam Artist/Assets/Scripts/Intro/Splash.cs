using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;

/***********************************
Script for Intro scene.
Used for playing video then logo.
***********************************/

public class Splash : MonoBehaviour
{
    [Tooltip("Logo Image")]
    public Image logo;
    [Tooltip("Load Scene Handler (SteamVR Load Level)")]
    public GameObject LoadSceneHandler;
    [Tooltip("Video Projector")]
    public GameObject videoplayer;
    [Tooltip("Video Player")]
    private VideoPlayer video;
    private bool logo_in = false, logo_out = false;
    private bool logo_in_s = false, logo_out_s = false;

    void Start()
    {
        video = videoplayer.GetComponent<VideoPlayer>();
        video.loopPointReached += EndReached;
        video.Play();
    }

    void Update()
    {
        if (!videoplayer.activeSelf && !logo_in_s)
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
            // Load Scene
            if (SteamVR.active)
            {
                // If VR
                LoadSceneHandler.SetActive(true);
            }
            else
            {
                // If non-VR
                Initiate.Fade("Menu", Color.black, 0.5f);
            }
        }
    }

    private IEnumerator FadeOut(Image i, float FadeTime)
    {
        while (i.color.a > 0)
        {
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.2f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == logo) { logo_out = true; }
    }

    private IEnumerator FadeIn(Image i, float FadeTime)
    {
        while (i.color.a < 1)
        {
            i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, i.transform.localPosition.z - 0.2f);
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / FadeTime));
            yield return null;
        }
        if (i == logo) { logo_in = true; }
    }

    void EndReached(VideoPlayer vp)
    {
        videoplayer.SetActive(false);
    }
}
