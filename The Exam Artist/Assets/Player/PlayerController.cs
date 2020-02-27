using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{

	public NavMeshAgent agent;
	// public Transform target; 
	public Vector3 destination;
	public Transform[] wayPoints;
	private int randomNumber;
	public int behaviour = 0; // moving
	public float speed = 2.0f;
	private int[] pathindex;
	private int idx = 0;  //point to the first index in pathindex
	private bool isWaiting;


	void Start()
	{
		// Cache agent component and destination
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		pathindex = GetIndexRandomNum(0, wayPoints.Length);
		print(wayPoints.Length);
		print(pathindex.Length);
		// StartCoroutine(Wait());



	}

	void Update()
	{
		MovingPlayer();
	}

	void MovingPlayer()
	{
		if (idx < pathindex.Length)
		{
			if (behaviour == 0)
			{
				behaviour = 1;
			}
			else
			{
				//int[] pathindex = GetIndexRandomNum(0, wayPoints.Length);

				//int idx = Random.Range(0, wayPoints.Length);
				if (behaviour == 1)
				{ // walk 
				  //Choose a random waypoint

					// Transform currentWaypoint=wayPoints[randomNumber];

					//go to selected waypoint
					Transform target = wayPoints[pathindex[idx]];
					Debug.Log(target.position);
					agent.SetDestination(target.position);

					if (Vector3.Distance(transform.position, target.position) < 1.0f)
					{
						agent.ResetPath();
						idx++;
						behaviour = Random.Range(1, 4);
						Debug.Log(target.position);
						Debug.Log(behaviour);
					}

				}
				else if (behaviour == 2)
				{  //idle

					//stop Movement
					agent.ResetPath();
					Debug.Log("idle");


					behaviour = Random.Range(1, 4);

					Debug.Log(behaviour);

				}
				else if (behaviour == 3)
				{  // watching

					//stop Movement
					agent.ResetPath();
					Debug.Log("Watching");
					behaviour = Random.Range(1, 4);

					Debug.Log(behaviour);
				}
			}
		}


	}

	public static int[] GetIndexRandomNum(int minValue, int maxValue)
	{
		System.Random random = new System.Random();
		int sum = Mathf.Abs(maxValue - minValue);//计算数组范围
		int site = sum;//设置索引范围
		int[] index = new int[sum];
		int[] result = new int[sum];
		int temp = 0;
		for (int i = minValue; i < maxValue; i++)
		{
			index[temp] = i;
			temp++;
		}
		for (int i = 0; i < sum; i++)
		{
			int id = random.Next(0, site - 1);
			result[i] = index[id];
			index[id] = index[site - 1];//因id随机到的数已经获取到了，用最后的一个数来替换它
			site--;//缩小索引范围
		}
		return result;
	}

	IEnumerator Wait()
	{
		isWaiting = true;  //set the bool to stop moving
		print("Start to wait");
		yield return new WaitForSeconds(20); // wait for 5 sec
		print("Wait complete");
		isWaiting = false; // set the bool to start moving

	}

}
