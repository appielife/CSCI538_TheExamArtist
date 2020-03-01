using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teacher: MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Alpha0)){
        ani.SetInteger("animation_int", 0);
        }
    if (Input.GetKeyDown(KeyCode.Alpha1)){
        ani.SetInteger("animation_int", 1);//walk
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
        ani.SetInteger("animation_int", 2);//lookaround
        }
    if (Input.GetKeyDown(KeyCode.Alpha3)){
        ani.SetInteger("animation_int", 3);//aware
        }
    if (Input.GetKeyDown(KeyCode.Alpha4)){
        int index = Random.Range(4,7);
        ani.SetInteger("animation_int", index);//angry
        }
    if (Input.GetKeyDown(KeyCode.Alpha5)){
        ani.SetInteger("animation_int", 8);//getout
        }

    }

}
