using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    //private float time = 25.0f;
    private GameObject obj;
    private TeacherController control;
    private Vector3 position;
    private Animator ani;
    private student target;
    private float timeLeft = 10.0f;
    private bool hit = false, moved = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void OnCollisionEnter(UnityEngine.Collision collision) { 
        if (collision.collider.tag == "Projectile" || collision.rigidbody.tag == "Projectile")
        {
            position = gameObject.transform.Find("Position").transform.position;
            obj = collision.collider.gameObject;
            control.setTarget(gameObject.transform.Find("Position").gameObject);
            hit = true;
        }
    }

    public bool isTrigger()
    {
        return hit;
    }

    private void Start()
    {
        ani = gameObject.transform.Find("student").GetComponent<Animator>();
        target = gameObject.transform.Find("student").GetComponent<student>();
        control = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        originalPosition = GameObject.FindGameObjectWithTag("Projectile").transform.position;
        originalRotation = GameObject.FindGameObjectWithTag("Projectile").transform.rotation;
    }

    private void Update()
    {
        if (hit)
        {
            if (!moved)
            {
                target.resetPosition();
                target.setPosition(new Vector3(-0.3f, 0.45f, 0.0f));
                target.setChairPosition(new Vector3(0.0f, 0.0f, -0.115f));
                moved = true;
            }
            target.setAnimation(11);
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                obj.transform.position = originalPosition;
                obj.transform.rotation = originalRotation;
                timeLeft = 10.0f;
                hit = false;
                moved = false;
                target.resetPosition();
                target.setAnimation(2);
            }
        }
    }
}
