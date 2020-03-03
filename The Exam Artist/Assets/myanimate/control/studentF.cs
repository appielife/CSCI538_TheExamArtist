using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class studentF : MonoBehaviour { 

    public Animator ani;
    private AudioSource[] source;
    private AudioSource[] teacher;
    public Random ran = new Random();

// Start is called before the first frame update
    void Start()
    {
        //control default status
        ani.SetInteger("animation_int", 6);
    }

    // Update is called once per frame
    void Update()
    {
    source = GameObject.FindGameObjectWithTag ("student").GetComponents<AudioSource> ();
    teacher = GameObject.FindGameObjectWithTag ("teacher").GetComponents<AudioSource> ();

    if (Input.GetKeyDown(KeyCode.Alpha0)){
        ani.SetInteger("animation_int", 0);//idle
        }
    if (Input.GetKeyDown(KeyCode.Alpha1)){
        int index = Random.Range(6,9);
        ani.SetInteger("animation_int", index);//sit
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
        int index = Random.Range(1,3);
        ani.SetInteger("animation_int", index);//write
        }
    if (Input.GetKeyDown(KeyCode.Alpha7)){
        ani.SetInteger("animation_int", 5);//depress
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        source[0].Play();

        }
    if (Input.GetKeyDown(KeyCode.Alpha8)){
        ani.SetInteger("animation_int", 4);//dance
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        source[0].Stop();
        source[1].Play();

        }
    
    // if (Input.GetKeyDown(KeyCode.Alpha5)){
    //     ani.SetInteger("animation_int", 9);//thumbup
    //     }
    }

    
    
}
