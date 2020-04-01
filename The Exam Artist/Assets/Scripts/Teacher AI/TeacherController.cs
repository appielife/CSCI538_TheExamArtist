using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class TeacherController : MonoBehaviour
{
    public float speed = 2.0f;
    public NavMeshAgent teacher;
    public GameObject student, gameoverTarget;
    public Animator ani;
    public Text text;
    public Random ran = new Random();
    public AudioClip wow;
    public TestPaperBehavior test;
    public IllegalMoveHandler illegalmove;
    
    private int behaviour = 1; // moving
    private bool inEyesight = false, gameover = false;
    private float minDistance = 10000f, minAngle = 120f, angle;
    private GameObject target;
    private AudioSource[] studentsound, teachersound;
    float timePaused = 0.0f;
    public float MaxPauseTime = 3.0f;

    private float timeLeft = 15.0f;

    void Start()
    {
        studentsound = GameObject.FindGameObjectWithTag("student").GetComponents<AudioSource>();
        teachersound = GameObject.FindGameObjectWithTag("teacher").GetComponents<AudioSource>();
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        timeLeft = setting.offset;
        teacher.speed = speed;
        if (setting.washroomed)
        {
            int i = Random.Range(1, 12);
            target = GameObject.Find("target" + i.ToString());
            teacher.transform.position = target.transform.position;
        }
        else
        {
            target = GameObject.Find("target1");
        }

    }

    void Update()
    {
        if (timeLeft > 0)
        {
            if (timeLeft < 13 && timeLeft > 12)
            {
                ani.SetInteger("animation_int", 9);
                teachersound[0].Play();
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
        //teacher.ResetPath();
        Debug.Log("back to move:" + " "+ destination.position);
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
                teacher.SetDestination(destination.position);
                Debug.Log("current destination:", destination);
            }

            if (target.GetComponent<PointFind>().nextPos && !gameover)
            {
                Debug.Log(teacher.transform.position+ ";"+ destination.position);
                if (Vector3.Distance(teacher.transform.position, destination.position) < 0.1f)
                {
                    teacher.ResetPath();
                    behaviour = 3;
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
        //string debug = "teacher pos" + teaPos + "student pos" + stuPos + angle.ToString() + "and" + distance.ToString();

        //if (distance < minDistance && angle < minAngle / 2)
        //{
        //    text.text = "in eyesight";
        //    inEyesight = true;
        //    if (illegalmove.illegal)
        //    {
        //        behaviour = 3;
        //    }
        //}
        //else
        //{
        //    text.text = "not in eyesight";
        //    inEyesight = false;
        //}
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
                Debug.Log("end pause" + behaviour);
            }
            else
            {
                //Debug.Log("still pause" + timePaused +","+ behaviour);
                int index = Random.Range(2, 3);
                ani.SetInteger("animation_int", index);
                teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
                teacher.SetDestination(teacher.transform.position);
                //teacher.ResetPath();
                
  
       
            }
            Debug.Log(behaviour);
        }

    }
}