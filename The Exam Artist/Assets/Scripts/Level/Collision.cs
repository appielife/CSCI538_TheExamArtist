using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************
Script for projectile collison on student
******************************************/

public class Collision : MonoBehaviour
{
    private GameObject obj;
    private TeacherController control;
    private Vector3 position;
    private Animator ani;
    private student target;
    private float timeLeft = 10.0f;
    private bool hit = false, moved = false, set = false, start = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private TestPaperBehavior test;

    private void OnCollisionEnter(UnityEngine.Collision collision) { 
        // If hit by projectile
        if (collision.collider.tag == "Projectile" || collision.rigidbody.tag == "Projectile")
        {
            position = gameObject.transform.Find("Position").transform.position; // Obtain student position
            obj = collision.collider.gameObject; // Obtain projectile
            if (start)
            {
                // If test start, set teacher's student target
                control.setTarget(gameObject.transform.Find("Position").gameObject);
            }
            hit = true;
        }
    }

    private void Start()
    {
        ani = gameObject.transform.Find("student").GetComponent<Animator>();    // Set student animator
        target = gameObject.transform.Find("student").GetComponent<student>();  // Set student target
        // Obtain scripts
        control = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
    }

    private void Update()
    {
        // If projectile position not set 
        if (!set)
        {
            originalPosition = GameObject.FindGameObjectWithTag("Projectile").transform.position;
            originalRotation = GameObject.FindGameObjectWithTag("Projectile").transform.rotation;
            set = true;
        }

        // If test not start, keep tracking
        if (!start)
        {
            start = test.isStart();
        }

        // If projectile hit
        if (hit)
        {
            if (start)
            {
                // If test start
                if (!moved)
                {
                    // Set hit student and chair position if not moved
                    target.resetPosition();
                    target.setPosition(new Vector3(-0.3f, 0.45f, 0.0f));
                    target.setChairPosition(new Vector3(0.0f, 0.0f, -0.215f));
                    moved = true;
                }
                target.setAnimation(11);    // Set animation

                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    // Reset to writing pose and positions
                    obj.transform.position = originalPosition;
                    obj.transform.rotation = originalRotation;
                    timeLeft = 10.0f;
                    hit = false;
                    moved = false;
                    target.resetPosition();
                    target.setAnimation(2);
                }
            }
            else
            {
                // If test not start, no action. Reset projectile position
                obj.transform.position = originalPosition;
                obj.transform.rotation = originalRotation;
                hit = false;
            }
        }
    }

    // Function to know if projectile hit
    public bool isTrigger()
    {
        return hit;
    }
}
