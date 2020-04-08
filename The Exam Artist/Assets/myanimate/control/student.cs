using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class student : MonoBehaviour { 

    public Animator ani;
    private AudioSource[] source;
    private AudioSource[] teacher;
    public Random ran = new Random();
    //public GameObject character;
    private bool notMoved = true;
    private float timeLeft = 15.0f;
    private int animationIndex = 2;

    public void setAnimation(int i)
    {
        animationIndex = i;
    }

    void Start()
    {
        source = GameObject.FindGameObjectWithTag ("student").GetComponents<AudioSource> ();
        teacher = GameObject.FindGameObjectWithTag ("teacher").GetComponents<AudioSource> ();

        ani.SetInteger("animation_int", 7);
        if (GameObject.Find("LevelSetting").GetComponent<LevelSetting>().washroomed)
        {
            timeLeft = -1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else{
            ani.SetInteger("animation_int", animationIndex);
        }
    // if (Input.GetKeyDown(KeyCode.Alpha0)){
    //     ani.SetInteger("animation_int", 0);//idle
    //     }
    // if (Input.GetKeyDown(KeyCode.Alpha1)){
    //     int index = Random.Range(7,10);
    //     ani.SetInteger("animation_int", index);//sit
    //     }
    // if (Input.GetKeyDown(KeyCode.Alpha2)){
    //         if(character != null && notMoved)
    //         {
    //             Debug.Log(character.transform.localPosition);
    //             character.transform.localPosition += new Vector3(0.7f, 0.0f, 0.0f);
    //             notMoved = false;
    //         }
    //     int index = Random.Range(1,3);
    //     ani.SetInteger("animation_int", index);//write
    //     }
    // if (Input.GetKeyDown(KeyCode.Alpha7)){
    //     ani.SetInteger("animation_int", 4);//depress
    //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
    //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
    //     source[0].Play();
    //     }
    // if (Input.GetKeyDown(KeyCode.Alpha8)){
    //     ani.SetInteger("animation_int", 6);//dance
    //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
    //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
    //     source[0].Stop();
    //     source[1].Play();
    //     }
    
 
    }
}
