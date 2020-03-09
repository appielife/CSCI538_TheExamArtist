using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

	public NavMeshAgent teacher;
	public GameObject student;
	public Animator ani;

	// public Transform target; 
	public Vector3 destination;
	GameObject target;
	public int behaviour = 0; // moving
	public float speed = 2.0f;
	float minDistance = 10000f;
	float minAngle = 120f;
	public Text text;
	public Random ran = new Random();


	
	void Start()
	{
		// Cache agent component and destination
		//teacher = GetComponent<NavMeshAgent>();
        //student = GetComponent<NavMeshAgent>();

		teacher.speed = speed;
		target = GameObject.Find("target1");
		ani.SetInteger("animation_int", 1);
		//Debug.Log(target.transform.position);
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

		eyesightCheck();
		
	}

	void Moving()
	{
		ani.SetInteger("animation_int", 1);
		if (target.GetComponent<PointFind>().nextPos)
		{
			Transform destination = target.transform;
			//Debug.Log("now destination:"+destination.position);
			teacher.SetDestination(destination.position);

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

		}
		else
		{
			text.text = "not in eyesight";
			//Debug.Log("out of eyesight");
		}
	}

	IEnumerator Pausing()
    {
		int index = Random.Range(2,3);
		ani.SetInteger("animation_int", index);
		//Debug.Log("Before Waiting 3 seconds");
		
		teacher.transform.Rotate(new Vector3(0, -30 * Time.deltaTime, 0));
		yield return new WaitForSeconds(3);
		
        //Debug.Log("After Waiting 3 Seconds");
		behaviour = 1;
 

    }

}
