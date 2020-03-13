using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject setting;
    private string hand = "LeftHand";

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
}
