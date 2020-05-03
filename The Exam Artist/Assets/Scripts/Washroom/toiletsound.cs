using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************
Script for sound control in washroom
***********************************/

public class toiletsound : MonoBehaviour
{
    [Tooltip("Audio source")]
    public AudioSource audioSource;
    [Tooltip("Audio 1")]
    public AudioClip otherClip1;
    [Tooltip("Audio 2")]
    public AudioClip otherClip2;
    [Tooltip("Audio 3")]
    public AudioClip otherClip3;
    [Tooltip("Audio 4")]
    public AudioClip otherClip4;

    private float musicVolume;
    private float randomNum;
    private int state;

    void Start () {
        musicVolume = 0.5f;
        randomPlay();
    }

    void Update () {
        audioSource.volume = musicVolume;
        if ((state == 1 && !audioSource.isPlaying)||(state == 2 && !audioSource.isPlaying) ||(state == 3 && !audioSource.isPlaying) ||(state == 4 && !audioSource.isPlaying)) { randomPlay(); }
    }

    // Function to randomly play audio
    void randomPlay()
    {
        randomNum = Random.Range(1.0f, 5.0f);
        if (randomNum >= 1.0f && randomNum < 2.0f) {state = 1; audioSource.clip = otherClip1; audioSource.Play(); }
        else if (randomNum >= 2.0f && randomNum < 3.0f) {state = 2; audioSource.clip = otherClip2; audioSource.Play(); }
        else if (randomNum >= 3.0f && randomNum < 4.0f) {state = 3; audioSource.clip = otherClip3; audioSource.Play(); }
        else if (randomNum >= 4.0f && randomNum <= 5.0f) {state = 4; audioSource.clip = otherClip4; audioSource.Play(); }
    }
}
