using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private float time = 25.0f;
    private GameObject obj;
    private TeacherController control;
    private Vector3 position;
    private Animator ani;
    private student target;
    private studentF targetF;
    private float timeLeft = 10.0f;
    private bool hit = false;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.name == "eraser")
        {
            position = gameObject.transform.Find("Position").transform.position;
            obj = collision.collider.gameObject;
            //Debug.Log(position);
            control.setTarget(gameObject.transform.Find("Position").gameObject);
            hit = true;
        }
    }

    private void Start()
    {
        ani = gameObject.transform.Find("student").GetComponent<Animator>();
        target = gameObject.transform.Find("student").GetComponent<student>();
        if(target == null)
        {
            targetF = gameObject.transform.Find("student").GetComponent<studentF>();
        }
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
            if (target)
            {
                target.setAnimation(11);
            }
            else
            {
                targetF.setAnimation(11);
            }
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                hit = false;
                if (target)
                {
                    target.setAnimation(2);
                }
                else
                {
                    targetF.setAnimation(1);
                }
            }
        }
    }
}
