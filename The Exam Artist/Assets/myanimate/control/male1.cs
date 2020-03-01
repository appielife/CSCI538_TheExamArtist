using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class male1 : MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
// Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(1,3);
        ani.SetInteger("animation_int", index);
    }

    // Update is called once per frame
    void Update()
    {
        int index = Random.Range(1,3);
    if (Input.GetKeyDown(KeyCode.Alpha0)){
        ani.SetInteger("animation_int", 0);
        }
    if (Input.GetKeyDown(KeyCode.Alpha1)){
        ani.SetInteger("animation_int", index);
        }
    if (Input.GetKeyDown(KeyCode.Alpha2)){
        ani.SetInteger("animation_int", 4);
        }
    if (Input.GetKeyDown(KeyCode.Alpha3)){
        ani.SetInteger("animation_int", 5);
        }
    


    }
    
}
