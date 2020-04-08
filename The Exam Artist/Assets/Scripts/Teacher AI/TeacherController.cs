using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class TeacherController : MonoBehaviour
{
    public float speed = 2.0f;
    public NavMeshAgent teacher;
    public GameObject student, gameoverTarget, giftTarget;
    public Animator ani;
    //public Text text;
    public Random ran = new Random();
    public AudioClip wow;
    public TestPaperBehavior test;
    public IllegalMoveHandler illegalmove;
    public GiftBlindEyesBehavior giftSkillTrigger;
    public ClapBombBehavior clapBombTrigger;

    private int behaviour = 1; // moving
    private bool inEyesight = false, gameover = false, play = false;
    private float angle;
    //private float minDistance = 10000f, minAngle = 120f
    private GameObject target;
    private AudioSource[] studentsound, teachersound;
    private float timePaused = 0.0f;
    float timeChecked = 0.0f;
    public float MaxPauseTime = 3.0f;
    public float MaxCheckTime = 5.5f;

    private float timeLeft = 15.0f;

    void Start()
    {
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        teacher.speed = speed;
        target = GameObject.Find("target1");
        target.transform.position = GetRandomPosition();
        
        setBribeTarget();

    }

    void Update()
    {
        if (timeLeft > 0)
        {
            if (!play)
            {
                ani.SetInteger("animation_int", 9);
                teachersound[0].Play();
                play = true;
            }
            timeLeft -= Time.deltaTime;
        }
        else
        {
            switch (behaviour)
            {
                case 0:
                    behaviour = 1;   //switch behavior from initialization to move
                    break;
                case 1:
                    Moving();  // move
                    break;
                case 2:
                case 3:
                    Pausing();
                    break;
                case 4:
                    teacher.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    behaviour = 2;
                    teachersound[1].Pause();
                    test.writeAnsToJson();
                    break;
                case 5:
                    bribeBehavior();
                    break;
                case 6:
                    checkStudentBehavior();
                    break;

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
        teacher.SetDestination(destination.position);
        //Debug.Log(destination.position);
        ani.SetInteger("animation_int", 1);
        if (gameover)
        {
            teacher.SetDestination(gameoverTarget.transform.position);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < 0.1f)
            {
                int index = Random.Range(5, 7);
                ani.SetInteger("animation_int", index);
                teacher.ResetPath();
                behaviour = 4;
            }
        }
        else
        {
            if (illegalmove.illegal && inEyesight)
            {
                teachersound[2].PlayOneShot(wow, 0.3f);
                gameover = true;
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else
            {
                if (clapBombTrigger.targetStudentPos != null)
                {
                    teacher.SetDestination(clapBombTrigger.targetStudentPos.transform.position);
                    behaviour = 6;
                }
                else if (giftSkillTrigger.isTrigger() == true)
                {
                    teacher.SetDestination(giftTarget.transform.position);
                    behaviour = 5;
                }
                else
                {
                    teacher.SetDestination(destination.position);
                    if (!teacher.hasPath)
                    {
                        target.transform.position = GetRandomPosition();
                        teacher.SetDestination(destination.position);
                    }
                }
            }

            if (!gameover)
            {
                if (Vector3.Distance(teacher.transform.position, destination.position) < 0.1f)
                {
                    //Debug.Log(teacher.transform.position);
                    teacher.ResetPath();
                    behaviour = 3;
                    target.transform.position = GetRandomPosition();
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
    }

    void Pausing()
    {
        timePaused += Time.deltaTime;
        teachersound[1].Pause();
        if (gameover)
        {
            //int index = Random.Range(5, 7);
            ani.SetInteger("animation_int", 3);
            behaviour = 1;
        }
        else
        {
            if (timePaused >= MaxPauseTime)
            {
                behaviour = 1;
                timePaused = 0.0f;
            }
            else
            {
                int index = Random.Range(2, 3);
                ani.SetInteger("animation_int", index);
                teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
                teacher.SetDestination(teacher.transform.position);
                //teacher.ResetPath();
            }
        }
    }
    
    void bribeBehavior()
    {
        if (giftSkillTrigger.isTrigger() == false)
        {
            behaviour = 1;
        }
        if (Vector3.Distance(teacher.transform.position, teacher.destination) < 0.1f)
        {
            teachersound[1].Pause();
            teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            ani.SetInteger("animation_int", 10);
        }
    }

    void Checking()
    {
        timeChecked += Time.deltaTime;
        teachersound[1].Pause();

        //ani.SetInteger("animation_int", 11);
        //Debug.Log("Before Waiting 3 seconds");

        if (timeChecked >= MaxCheckTime)
        {
            behaviour = 1;
            clapBombTrigger.targetStudentPos = null;
            timeChecked = 0.0f;
        }
        else
        {
            ani.SetInteger("animation_int", 10);
        }
    }

    void checkStudentBehavior()
    {
        if (Vector3.Distance(teacher.transform.position, teacher.destination) < 0.1f)
        {
            teachersound[1].Pause();
            teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            Checking();
        }
    }

    public void setTarget(GameObject t)
    {
        target = t;
    }

    void setBribeTarget()
    {
        int index = Random.Range(1, 11);
        giftTarget = GameObject.FindGameObjectsWithTag("StudentPosition")[index];
    }

    public Vector3 GetRandomPosition()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);
        //point = new Vector3(-4.2f, -0.8f, -0.7f);
        //Debug.Log("generate: " + point);
        return point;
    }
}