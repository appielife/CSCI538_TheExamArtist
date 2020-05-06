using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/*************************************************
Script to control female who holds answer outside
*************************************************/

[RequireComponent(typeof(Animator))]
public class femaleoutside : MonoBehaviour
{
    private Animator ani;
    private Random ran = new Random();
    float m_Speed;

    private void Awake()
    {
        ani = gameObject.GetComponent<Animator>();
    }

    void OnEnable()
    {
        m_Speed = 5.0f;
        ani.SetInteger("animation_int", 1);
    }

    void Update()
    {
        if (transform.position.z > -10)
        {
            transform.Translate(Vector3.right * Time.deltaTime * m_Speed, Space.Self);
        }
        else
        {
            var v = transform.localPosition;
            v.z = 18.2f;
            transform.localPosition = v;
            gameObject.GetComponent<femaleoutside>().enabled = false;
            ani.SetInteger("animation_int", 0);
        }
    }

    public void startAnimation()
    {
        ani.SetInteger("animation_int", 1);
    }

}
