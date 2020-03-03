using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class studentF : MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
// Start is called before the first frame update
    void Start()
    {
        //control default status
        int index = Random.Range(6,9);
        ani.SetInteger("animation_int", index);
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Alpha0)){
        ani.SetInteger("animation_int", 0);//idle
        }
    if (Input.GetKeyDown(KeyCode.Alpha1)){
        int index = Random.Range(1,3);
        ani.SetInteger("animation_int", index);//write
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
        ani.SetInteger("animation_int", 5);//depress
        }
    // if (Input.GetKeyDown(KeyCode.Alpha3)){
    //     ani.SetInteger("animation_int", 4);//dance
    //     }
    if (Input.GetKeyDown(KeyCode.Alpha4)){
        int index = Random.Range(6,9);
        ani.SetInteger("animation_int", index);//sit
        }
    // if (Input.GetKeyDown(KeyCode.Alpha5)){
    //     ani.SetInteger("animation_int", 9);//thumbup
    //     }
    }

    
    
}
