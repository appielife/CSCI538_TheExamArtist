using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class male1 : MonoBehaviour { 

    public Animator ani;
// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Alpha0))

    {

        idle();

    }

    if (Input.GetKeyDown(KeyCode.Alpha1))
    {

            dance();

    }

    if (Input.GetKeyDown(KeyCode.Alpha2))

    {

        write1();

    }

    if (Input.GetKeyDown(KeyCode.Alpha3))

    {

        write2();

    }
    if (Input.GetKeyDown(KeyCode.Alpha4))

    {

        write3();

    }

    if (Input.GetKeyDown(KeyCode.Alpha5))

        {

            walk();

        }




    }
    private void idle()

    {

        ani.SetInteger("animation_int", 0);

    }


    private void dance()
    {

        ani.SetInteger("animation_int", 1);

    }


    private void write1()

    {

        ani.SetInteger("animation_int", 2);

    }

    private void write2()

    {

        ani.SetInteger("animation_int", 3);

    }

    private void write3()

    {

        ani.SetInteger("animation_int", 4);

    }
    private void walk()

    {

        ani.SetInteger("animation_int", 5);

    }
}
