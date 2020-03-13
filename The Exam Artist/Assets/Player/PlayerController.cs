using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Valve.VR;
using UnityEngine.SceneManagement;


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
    private float angle;
    private bool gameover = false;
    private GameObject temp;
    private AudioSource[] studentsound;
    private AudioSource[] teachersound;
    public AudioClip wow;
    public TestPaperBehavior test;


    private float timeLeft = 5.0f;

    void Start()
    {
        // Cache agent component and destination
        //teacher = GetComponent<NavMeshAgent>();
        //student = GetComponent<NavMeshAgent>();


        teacher.speed = speed;
        target = GameObject.Find("target1");
        ani.SetInteger("animation_int", 0);
        //Debug.Log(target.transform.position);

        new_mv = mv.GetComponent<IllegalMoveHandler>();
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            //Debug.Log(behaviour);
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
            else if (behaviour == 4)
            {
                teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                behaviour = 2;
                test.writeAnsToJson();
                FadeIn();
                Invoke("FadeOut", 2.0f);
            }
            if (!gameover)
            {
                eyesightCheck();
            }
        }
    }

    void Moving()
    {
        if (!teachersound[1].isPlaying)
        {
            teachersound[1].Play();
        }
        else
        {
            teachersound[1].UnPause();
        }
        Transform destination = target.transform;
        ani.SetInteger("animation_int", 1);
        if (gameover)
        {
            teacher.SetDestination(gameoverTarget.transform.position);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < 1.0f)
            {
                int index = Random.Range(5, 7);
                ani.SetInteger("animation_int", index);
                teacher.ResetPath();
                behaviour = 4;
            }
        }
        else
        {
            //Debug.Log("INHERE");
            if (new_mv.illegal && inEyesight)
            {
                teachersound[2].PlayOneShot(wow, 0.3f);
                gameover = true;
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else
            {
                teacher.SetDestination(destination.position);
            }
            if (target.GetComponent<PointFind>().nextPos && !gameover)
            {
                if (Vector3.Distance(teacher.transform.position, destination.position) < 1.0f)
                {
                    teacher.ResetPath();
                    behaviour = 3;
                    //Debug.Log(destination.position);
                    //Debug.Log(behaviour);

                    target = target.GetComponent<PointFind>().nextPos;  // target赋值为下一个点的坐标
                }
            }
        }
    }

    void eyesightCheck()
    {
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
            //Debug.Log("in eyesight");
            inEyesight = true;
            if (new_mv.illegal)
            {
                //Debug.Log("in eyesight");
                behaviour = 3;
            }

        }
        else
        {
            text.text = "not in eyesight";
            //Debug.Log("out of eyesight");
            inEyesight = false;
        }
    }

    IEnumerator Pausing()
    {

        teachersound[1].Pause();
        if (gameover)
        {
            //int index = Random.Range(5, 7);
            ani.SetInteger("animation_int", 3);
            yield return new WaitForSeconds(3);

            Debug.Log("Before Waiting 3 seconds");
            behaviour = 1;
        }
        else
        {
            int index = Random.Range(2, 3);
            ani.SetInteger("animation_int", index);
            //Debug.Log("Before Waiting 3 seconds");

            teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
            yield return new WaitForSeconds(3);

            //Debug.Log("After Waiting 3 Seconds");
            behaviour = 1;
        }

    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0.0f);
        SteamVR_Fade.Start(Color.black, 2.0f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0.0f);
        SteamVR_Fade.Start(Color.clear, 2.0f);
        SceneManager.LoadScene(2);
    }

}
