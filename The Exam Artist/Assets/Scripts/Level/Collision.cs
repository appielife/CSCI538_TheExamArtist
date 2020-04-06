using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private float time = 25.0f;
    private GameObject obj;
    private TeacherController control;
    private Vector3 position;
    public Animator ani;
    private float timeLeft = 10.0f;
    private bool hit = false;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.name == "Something")
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            position = parent.transform.Find("Position").transform.position;
            obj = collision.collider.gameObject;
            Debug.Log(position);
            control.setTarget(parent.transform.Find("Position").gameObject);
            hit = true;
        }
    }

    private void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        control = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            Destroy(obj);
        }
        if (hit)
        {
            ani.SetInteger("animation_int", 11);
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                hit = false;
            }
        }
    }
}
