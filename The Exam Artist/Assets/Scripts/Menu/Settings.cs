using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************
Script for store settings from Menu scene
****************************************/

public class Settings : MonoBehaviour
{
    public GameObject setting;
    private string hand = "LeftHand";
    private int volume = 50;
    private bool failed = false;
    public AudioSource click;

    void Start()
    {
        DontDestroyOnLoad(setting);
    }

    public void setHand(string hand)
    {
        this.hand = hand;
    }

    public string getHand()
    {
        return hand;
    }

    public void setVolume(int volume)
    {
        this.volume = volume;
    }

    public int getVolume()
    {
        return volume;
    }

    public void setFailed(bool failed)
    {
        this.failed = failed;
    }

    public bool getFailed()
    {
        return failed;
    }
}
