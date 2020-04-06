using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public Settings setting;
    private AudioSource[] sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponents<AudioSource>();
        setting = GameObject.Find("Settings").GetComponent<Settings>();
        for(int i = 0; i < sound.Length; i++){
            sound[i].volume = sound[i].volume * (((setting.getVolume() - 50) / 50) + 1);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
