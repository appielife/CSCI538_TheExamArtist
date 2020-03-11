using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;



public class PlayerController : MonoBehaviour
{

    public NavMeshAgent teacher;
    public GameObject student;
    public Animator ani;

    // public Transform target; 
    public Vector3 destination;
    public GameObject gameoverTarget;
    GameObject target;
    public int behaviour = 0; // moving
    public float speed = 2.0f;
    float minDistance = 10000f;
    float minAngle = 120f;
    public Text text;
    public Random ran = new Random();
    public bool inEyesight = false;
    public GameObject mv;
    private IllegalMoveHandler new_mv = null;
    private AudioSource[] studentsound;
    private float angle;
    private bool test = false;
    private GameObject temp;

    void Start()
    {
        // Cache agent component and destination
        //teacher = GetComponent<NavMeshAgent>();
        //student = GetComponent<NavMeshAgent>();
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teacher.speed = speed;
        target = GameObject.Find("target1");
        ani.SetInteger("animation_int", 1);
        //Debug.Log(target.transform.position);

        new_mv = mv.GetComponent<IllegalMoveHandler>();

    }

    void Update()
    {

        if (behaviour == 0) // start status, set status as 1 to moving
        {
            behaviour = 1;
        }
        else if (behaviour == 1) // moving status
        {
            Moving();
        }
        else if (behaviour == 2) // idle status
        {

        }
        else if (behaviour == 3) // pausing and watching status
        {
            //Debug.Log("Pausing");
            StartCoroutine(Pausing());
            //behaviour = 1;
        }
        else if(behaviour == 4)
        {
            /*int index = Random.Range(2, 3);
            ani.SetInteger("animation_int", index);*/
            //teacher.transform.Rotate(new Vector3(0, angle, 0));
            teacher.transform.eulerAngles = new Vector3(0f,90f,0f);
            behaviour = 2;
            FadeIn();
            Invoke("FadeOut", 2.0f);
        }

        eyesightCheck();

    }

    void Moving()
    {
        Transform destination = target.transform;
        ani.SetInteger("animation_int", 1);
        if (test)
        {
            teacher.SetDestination(gameoverTarget.transform.position);
            // ani.SetInteger("animation_int", 1);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < 1.0f)
            {
                int index = Random.Range(2, 3);
                ani.SetInteger("animation_int", index);
                teacher.ResetPath();
                behaviour = 4;
            }
        }
        else
        {
            if (new_mv.illegal && inEyesight) // If illegal and in eye sight, stop 
            {
                test = true;
                //teacher.SetDestination(teacher.transform.position);
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else // Continue to walk
            {
                //test = false;
                //target = temp;
                //Debug.Log("now destination:"+destination.position);
                teacher.SetDestination(destination.position);
            }
            if (target.GetComponent<PointFind>().nextPos && !(new_mv.illegal && inEyesight))
            {
                if (Vector3.Distance(teacher.transform.position, destination.position) < 1.0f)
                {
                    teacher.ResetPath();
                    behaviour = 3;
                    //Debug.Log(destination.position);
                    //Debug.Log(behaviour);

                    target = target.GetComponent<PointFind>().nextPos;  // target赋值为下一个点的坐标
                                                                        //temp = target;
                }
            }
        }
        
    }

    void eyesightCheck()
    {
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        Vector3 teaPos = transform.position;
        Vector3 stuPos = student.transform.position;
        float distance = Vector3.Distance(teaPos, stuPos);
        Vector3 srcLocalVect = stuPos - teaPos;
        srcLocalVect.y = 0;
        Vector3 forwardLocalPos = teacher.transform.forward * 1 + teaPos;
        Vector3 forwardLocalVect = forwardLocalPos - teaPos;
        forwardLocalVect.y = 0;
        float angle = Vector3.Angle(srcLocalVect, forwardLocalVect);
        //Debug.Log(angle.ToString() + distance.ToString());
        string debug = "teacher pos" + teaPos + "student pos" + stuPos + angle.ToString() + "and" + distance.ToString();

        if (distance < minDistance && angle < minAngle / 2)
        {
            text.text = "in eyesight";
            inEyesight = true;
            //Debug.Log(new_mv.illegal);
            if (new_mv.illegal)
            {
                // student is in eyesight and is in illegal move 
                studentsound[2].Play();
                behaviour = 3;

                //Debug.Log("In eyesight and illegal move");
            }
        }
        else
        {
            text.text = "not in eyesight";
            studentsound[2].Stop();
            inEyesight = false;

            //Debug.Log("out of eyesight");
            //studentsound[2].Stop();
        }
    }

    IEnumerator Pausing()
    {


        //Debug.Log("Before Waiting 3 seconds");
        if (new_mv.illegal && inEyesight)
        {
            int index = Random.Range(5, 7);

            ani.SetInteger("animation_int", index);
            Debug.Log(angle);
            // teacher.transform.Rotate(new Vector3(0, -angle * Time.deltaTime, 0));
            yield return new WaitForSeconds(3);
        }
        else
        {
            int index = Random.Range(2, 3);
            ani.SetInteger("animation_int", index);
            teacher.transform.Rotate(new Vector3(0, 30 * Time.deltaTime, 0));
            yield return new WaitForSeconds(3);
            behaviour = 1;
        }
    }



    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.Start(Color.black, 10f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, 2f);
        SceneManager.LoadScene(4);
    }

}
