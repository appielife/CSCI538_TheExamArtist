using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************** 
Script for Rotating Object
**************************/

public class Rotation : MonoBehaviour
{
    [Tooltip("Rotate around target object")]
    public Transform target;
    [Tooltip("Rotation degree per second")]
    public int degree;
    private Vector3 point;

    void Start()
    {
        point = target.position;
    }

    void Update()
    {
        transform.RotateAround(point, Vector3.up, degree * Time.deltaTime);
    }
}
