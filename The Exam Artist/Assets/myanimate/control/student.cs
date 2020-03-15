using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class student : MonoBehaviour { 

    public Animator ani;
    private AudioSource[] source;
    private AudioSource[] teacher;
    public Random ran = new Random();
<<<<<<< HEAD
=======
    public GameObject character;
    private bool notMoved = true;
>>>>>>> 3e4a7def4b33fe39b56bc27272105f6517cd6559
// Start is called before the first frame update
    void Start()
    {
        //control default status
        // int index = Random.Range(7,10);
        // ani.SetInteger("animation_int", index);
        ani.SetInteger("animation_int", 7);
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
        int index = Random.Range(7,10);
        ani.SetInteger("animation_int", index);//sit
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
<<<<<<< HEAD
=======
            if(character != null && notMoved)
            {
                Debug.Log(character.transform.localPosition);
                character.transform.localPosition += new Vector3(0.7f, 0.0f, 0.0f);
                notMoved = false;
            }
>>>>>>> 3e4a7def4b33fe39b56bc27272105f6517cd6559
        int index = Random.Range(1,3);
        ani.SetInteger("animation_int", index);//write
        }
    if (Input.GetKeyDown(KeyCode.Alpha7)){
        ani.SetInteger("animation_int", 4);//depress
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        source[0].Play();
        }
    if (Input.GetKeyDown(KeyCode.Alpha8)){
        ani.SetInteger("animation_int", 6);//dance
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        source[0].Stop();
        source[1].Play();
        }
    
 
    }
}
