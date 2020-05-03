using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************
Script for projectile collison on student
******************************************/

public class CollisionFloor : MonoBehaviour
{
    private GameObject obj;
    private bool hit = false, set = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private TeacherController control;
    private float timeLeft = 3.0f;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // If hit by projectile
        if (collision.collider.tag == "Projectile")
        {
            obj = collision.collider.gameObject; // Obtain projectile
            hit = true;
        }
    }

    private void Start()
    {
        originalPosition = GameObject.FindGameObjectWithTag("Projectile").transform.position;   // Set original position
        originalRotation = GameObject.FindGameObjectWithTag("Projectile").transform.rotation;   // Set original rotation
        control = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
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

        // If projectile hit floor
        if (hit)
        {
            // If not projectile hit student
            if (!control.collision)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                }
                else
                {
                    // Reset projectile position
                    obj.transform.position = originalPosition;
                    obj.transform.rotation = originalRotation;
                    hit = false;
                    timeLeft = 3.0f;
                }
            }
        }
    }
}
