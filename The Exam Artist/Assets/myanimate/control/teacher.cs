using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teacher: MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
    private AudioSource source;
    public AudioClip yell;
// Start is called before the first frame update
    void Start()
    {
        ani.SetInteger("animation_int", 9);
        source = GameObject.FindGameObjectWithTag ("teacher").GetComponent<AudioSource> ();
        source.PlayOneShot(yell,0.2F);
    }

    // Update is called once per frame
    void Update()
    {
        source = GameObject.FindGameObjectWithTag ("teacher").GetComponent<AudioSource> ();
    if (Input.GetKeyDown(KeyCode.Alpha0)){
        ani.SetInteger("animation_int", 0);
        source.Pause();
        }
    if (Input.GetKeyDown(KeyCode.Alpha1)){
        ani.SetInteger("animation_int", 1);//walk
        source.Play();
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
        ani.SetInteger("animation_int", 2);//lookaround
        source.Pause();
        }
    if (Input.GetKeyDown(KeyCode.Alpha3)){
        ani.SetInteger("animation_int", 3);//aware
        source.Pause();
        }
    if (Input.GetKeyDown(KeyCode.Alpha4)){
        int index = Random.Range(4,7);
        ani.SetInteger("animation_int", index);//angry
        source.Pause();
        }
    if (Input.GetKeyDown(KeyCode.Alpha5)){
        ani.SetInteger("animation_int", 8);//getout
        source.Pause();
        }
        

    }

}
