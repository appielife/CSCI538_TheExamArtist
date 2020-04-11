using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class studentF : MonoBehaviour { 

    public Animator ani;
    private GameObject chair;
    private AudioSource[] source;
    private AudioSource[] teacher;
    public Random ran = new Random();
    private float timeLeft = 15.0f;
    private int animationIndex = 1;
    private bool writing = false;
    private Vector3 original, chairOriginal;

    public void setAnimation(int i)
    {
        animationIndex = i;
    }

    void Start()
    {
        source = GameObject.FindGameObjectWithTag ("student").GetComponents<AudioSource> ();
        teacher = GameObject.FindGameObjectWithTag ("teacher").GetComponents<AudioSource> ();

        //control default status
        ani.SetInteger("animation_int", 7);
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        if (setting.washroomed)
        {
            timeLeft = -1.0f;
        }
        original = gameObject.transform.localPosition;
        chair = gameObject.transform.parent.transform.Find("prop_sch_tablechair").transform.Find("prop_sch_chair").gameObject;
        chairOriginal = chair.transform.localPosition;
    }
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else{
            if (!writing)
            {
                //setPosition(new Vector3(0.2f, 0.4f, 0.0f));
                //setChairPosition(new Vector3(0.0f, 0.0f, -0.115f));
                setPosition(new Vector3(0.5f, - 0.05f, 0.0f));
                writing = true;
            }
            ani.SetInteger("animation_int", animationIndex);
        }

        // if (Input.GetKeyDown(KeyCode.Alpha0)){
        //     ani.SetInteger("animation_int", 0);//idle
        //     }
        // if (Input.GetKeyDown(KeyCode.Alpha1)){
        //     int index = Random.Range(6,9);
        //     ani.SetInteger("animation_int", index);//sit
        //     }
        // if (Input.GetKeyDown(KeyCode.Alpha2)){
        //     int index = Random.Range(1,3);
        //     ani.SetInteger("animation_int", index);//write
        //     }
        // if (Input.GetKeyDown(KeyCode.Alpha7)){
        //     ani.SetInteger("animation_int", 5);//depress
        //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        //     source[0].Play();

        //     }
        // if (Input.GetKeyDown(KeyCode.Alpha8)){
        //     ani.SetInteger("animation_int", 4);//dance
        //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[0].Stop();
        //     GameObject.FindGameObjectWithTag ("backgroundmusic").GetComponents<AudioSource> ()[1].Stop();
        //     source[0].Stop();
        //     source[1].Play();

        //     }

    }

    public void setPosition(Vector3 position)
    {
        gameObject.transform.localPosition += position;
    }

    public void setChairPosition(Vector3 position)
    {
        chair.transform.localPosition += position;
    }

    public void resetPosition()
    {
        gameObject.transform.localPosition = original;
        chair.transform.localPosition = chairOriginal;
        writing = false;
    }



}
