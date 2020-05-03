using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************
Script for female jogging outside
*********************************/

[RequireComponent(typeof(Animator))]
public class jog : MonoBehaviour { 

    private Animator ani;

    private void Awake()
    {
        ani = gameObject.GetComponent<Animator>();
    }

    void OnEnable()
    {
        ani.SetInteger("animation_int", 10);
    }
}
