using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

[RequireComponent(typeof(Animator))]
public class male1 : MonoBehaviour
{
    [Tooltip("Hallway Door")]
    public GameObject door;
    private Animator ani;
    private Random ran = new Random();

    Quaternion rotationMin = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    Quaternion rotationMax = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
    Quaternion rotation;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        ani.SetInteger("animation_int", 1);
        rotation = door.transform.rotation;
    }

    void Update()
    {
        if (transform.localPosition.z < 17.45f)
        {
            if (transform.localPosition.z > 9.32f && transform.localPosition.z < 12.95f)
            {
                if (rotation.y < rotationMax.y)
                {
                    ani.SetInteger("animation_int", 0);
                    rotation.eulerAngles = new Vector3(0.0f, rotation.eulerAngles.y + 30.0f * Time.deltaTime, 0.0f);
                    door.transform.rotation = rotation;
                }
                else
                {
                    if (door.transform.eulerAngles.y > 90)
                    {
                        Vector3 temp = door.transform.eulerAngles;
                        temp.y = 90.0f;
                        door.transform.eulerAngles = temp;
                    }
                    ani.SetInteger("animation_int", 1);
                    transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
                }
            }
            else if (transform.localPosition.z > 12.95f)
            {
                if (rotation.y > rotationMin.y)
                {
                    rotation.eulerAngles = new Vector3(0.0f, rotation.eulerAngles.y - 30.0f * Time.deltaTime, 0.0f);
                    door.transform.rotation = rotation;
                }
                else
                {
                    if (door.transform.eulerAngles.y - 360 < 0)
                    {
                        Vector3 temp = door.transform.eulerAngles;
                        temp.y = 0.0f;
                        door.transform.eulerAngles = temp;
                    }
                    transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
                }
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            var v = transform.localPosition;
            v.z = -17.45f;
            transform.localPosition = v;
        }
    }
}
