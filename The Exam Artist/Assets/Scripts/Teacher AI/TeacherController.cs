using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/******************************************
Script of teacher behavior in Level scene
******************************************/

[RequireComponent(typeof(Animator))]
public class TeacherController : MonoBehaviour
{
    [Tooltip("Set speed of teacher")]
    public float speed = 2.0f;
    [Tooltip("Set the student position (table/chair)")]
    public GameObject student;
    [Tooltip("Set game over position")]
    public GameObject gameoverTarget;
    [Tooltip("Audio for getting caught")]
    public AudioClip wow;
    [Tooltip("Maximum pausing second")]
    public float MaxPauseTime = 3.0f;

    [HideInInspector]
    public bool collision = false;
    [HideInInspector]
    public bool gameover = false;

    private NavMeshAgent teacher;
    private Animator ani;
    private IllegalMoveHandler illegalmove;
    private GiftBlindEyesBehavior giftSkillTrigger;
    private TimeFreezeBehavior freezeSkillTrigger;
    private TestPaperBehavior test;
    private Settings settings;
    private GameObject target, giftTarget;
    private AudioSource[] studentsound, teachersound;

    private int behaviour = 1;
    private bool inEyesight = false, play = false;
    // Current angle, minimum reach distance, teacher FOV, teacher eyesight distance
    private float angle, minDistance = 0.2f, minAngle = 120f, minEyesight = 10000f;

    // Counter for time paused, starting time offset
    private float timePaused = 0.0f, timeLeft = 12.0f;

    void Start()
    {
        // Obtain audio
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();

        // Obtain scripts
        illegalmove = GameObject.Find("IllegalMoveHandler").GetComponent<IllegalMoveHandler>();
        giftSkillTrigger = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();
        freezeSkillTrigger = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();

        // Obtain starting time offset
        LevelSetting levelsetting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = levelsetting.offset;

        // Obtain starting time offset
        if (GameObject.Find("Settings"))
        {
            settings = GameObject.Find("Settings").GetComponent<Settings>();
        }

        teacher = gameObject.GetComponent<NavMeshAgent>();  // Set NavMeshAgent
        teacher.speed = speed;                              // Set teacher speed

        target = new GameObject();                          // Set target object
        target.name = "target";        
        target.transform.position = GetRandomPosition();

        ani = gameObject.GetComponent<Animator>();          // Set animator
    }

    void Update()
    {
        if (!test.onPrepare)    // If not preparing
        {
            if (timeLeft > 0)   // If starting time offset > 0
            {
                if (!play)      // If haven't start talking, play talking sound
                {
                    ani.SetInteger("animation_int", 9);
                    teachersound[0].Play();
                    play = true;
                }
                timeLeft -= Time.deltaTime;
            }
            else                // If starting time offset < 0
            {
                if (!gameover)  // Check eyesight if haven't gameover
                {
                    eyesightCheck();
                }

                switch (behaviour)
                {
                    case 1:     // Move action
                        Moving(); 
                        break;
                    case 2:     // Null action
                        break;
                    case 3:     // Pause action
                        Pausing();
                        break;
                    case 4:     // Game over action
                        if (settings) {
                            settings.setFailed(true);
                        }
                        teacher.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                        teachersound[1].Pause();     
                        test.writeAnsToJson();
                        behaviour = 2;
                        break;
                    case 5:     // Bribe skill action
                        bribeBehavior();
                        break;
                }
            }
        }
    }

    // Function for move action
    void Moving()
    {
        // If freezing skill not activated
        if (!freezeSkillTrigger.isExisting())
        {
            //Play high heel sound
            if (!teachersound[1].isPlaying)
            {
                teachersound[1].Play(); 
            }
            else
            {
                teachersound[1].UnPause();
            }
        }
        else
        {
            // Pause high heel sound if freeze
            teachersound[1].Pause();
        }

        Transform destination = target.transform;   // Set destination
        ani.SetInteger("animation_int", 1);         // Set walk animation

        if (gameover)
        {
            // If game over, walk to game over target
            teacher.SetDestination(gameoverTarget.transform.position);
            if (Vector3.Distance(teacher.transform.position, teacher.destination) < minDistance)
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
                // If illegal move and in eyesight
                teacher.SetDestination(gameoverTarget.transform.position);
            }
            else
            {
                if (giftSkillTrigger.isTrigger() == true)
                {
                    // If bribe skill activated
                    setBribeTarget();
                    teacher.SetDestination(giftTarget.transform.position);
                    behaviour = 5;
                }
                else
                {
                    // If nothing activated
                    teacher.SetDestination(destination.position);
                    if (!teacher.hasPath && !collision)
                    {
                        target.transform.position = GetRandomPosition();
                    }
                }
            }

            if (!gameover)
            {
                // Move to target if not game over
                if (Vector3.Distance(teacher.transform.position, destination.position) < minDistance)
                {
                    teacher.ResetPath();
                    behaviour = 3;
                    target.transform.position = GetRandomPosition();
                }
            }
        }
    }

    // Function for eyesight checking
    void eyesightCheck()
    {
        Vector3 teaPos = transform.position;
        Vector3 stuPos = student.transform.position;
        float distance = Vector3.Distance(teaPos, stuPos);  // Distance between student and teacher

        Vector3 srcLocalVect = stuPos - teaPos;
        srcLocalVect.y = 0;
        Vector3 forwardLocalPos = teacher.transform.forward * 1 + teaPos;
        Vector3 forwardLocalVect = forwardLocalPos - teaPos;// Normal of teacher
        forwardLocalVect.y = 0;
        float angle = Vector3.Angle(srcLocalVect, forwardLocalVect);

        // If distance in teacher's eyesight and teacher's FOV
        if (distance < minEyesight && angle < minAngle / 2)
        {
            inEyesight = true;
            // If currently illegal movement
            if (illegalmove.illegal)
            {
                teachersound[2].PlayOneShot(wow, 0.3f);
                gameover = true;
                test.gameOver();
                behaviour = 3;
                teacher.speed = 2.0f;
                MaxPauseTime = 3.0f;
            }
        }
        else
        {
            inEyesight = false;
        }
    }

    // Function for eyesight checking
    void Pausing()
    {
        teachersound[1].Pause(); // Pause high heel sound

        // If freezing skill not activated
        if (!freezeSkillTrigger.isExisting())
        {
            timePaused += Time.deltaTime;

            if (gameover)
            {
                // If game over, stop pausing and move to game over target
                ani.SetInteger("animation_int", 3);
                behaviour = 1;
            }
            else
            {
                if (timePaused >= MaxPauseTime)
                {
                    // If paused enough, start moving to next target
                    behaviour = 1;
                    timePaused = 0.0f;
                    if (collision)
                    {
                        // If pause by projectile, restore normal pause settings
                        collision = false;
                        teacher.speed = 2.0f;
                        MaxPauseTime = 3.0f;
                    }
                }
                else
                {
                    if (collision)
                    {
                        // If pause by projectile
                        teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                        ani.SetInteger("animation_int", 10);
                    }
                    else
                    {
                        // If normal pause
                        int index = Random.Range(2, 3);
                        ani.SetInteger("animation_int", index);
                        teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
                    }
                    teacher.SetDestination(teacher.transform.position);
                }
            }
        }
    }

    // Function to set bribe target
    void setBribeTarget()
    {
        giftTarget = GameObject.Find("Student" + giftSkillTrigger.target).transform.Find("Position").gameObject;
    }

    // Function for bribe behavior
    void bribeBehavior()
    {
        if (giftSkillTrigger.isTrigger() == false)
        {
            // If bribe time up
            behaviour = 1;
        }

        // Walk to selected bribe location
        if (Vector3.Distance(teacher.transform.position, teacher.destination) < minDistance)
        {
            teachersound[1].Pause();
            teacher.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            ani.SetInteger("animation_int", 10);
        }
    }

    // Function for set target when projectile hit
    public void setTarget(GameObject t)
    {
        // If not game over
        if (!gameover)
        {
            target.transform.position = t.transform.position;
            behaviour = 1;
            teacher.speed = 4.0f;
            MaxPauseTime = 5.0f;
            collision = true;
        }
    }

    // Function to generate random position on navmesh
    public Vector3 GetRandomPosition()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int t = Random.Range(0, navMeshData.indices.Length - 3);
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);
        //Debug.Log("generate: " + point);
        return point;
    }
}