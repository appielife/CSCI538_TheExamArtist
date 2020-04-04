using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private float time = 25.0f;
    private GameObject obj;
    private TeacherController control;
    private Vector3 position;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.name == "Something")
        {
            GameObject parent = gameObject.transform.parent.gameObject;
            position = parent.transform.Find("Position").transform.position;
            obj = collision.collider.gameObject;
            Debug.Log(position);
            control.setTarget(parent.transform.Find("Position").gameObject);
            
        }
    }

    private void Start()
    {
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
    }
}
