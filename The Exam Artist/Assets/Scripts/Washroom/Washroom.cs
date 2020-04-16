using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Washroom : MonoBehaviour
{
    public LevelSetting setting;
    public GameObject LoadSceneHandler;
    private float timer;
    private GameObject paper;
    void Start()
    {
        List<int> randomArray = new List<int>();
        if (GameObject.Find("LevelSetting") != null)
        {
            setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
            timer = setting.washroomDuration;
            paper = GameObject.Find("Paper");

            int time = (int)Mathf.Ceil((float)setting.timeLeft / 60.0f);
            string text = "";

            if (setting.timeLeft > (float)setting.initialTime / 2.0f)
            {
                for (int i = 0; i < time; i++)
                {
                    randomArray.Add(0);
                }
                randomArray.Add(1);
            }
            else
            {
                for (int i = 0; i < time + (int)Mathf.Ceil((float)setting.initialTime / 120.0f); i++)
                {
                    randomArray.Add(1);
                }
                randomArray.Add(2);
            }
            //Debug.Log(randomArray);
            int numQues = randomArray[Random.Range(0, randomArray.Count)];

            List<int> counter = new List<int>();
            for (int i = 0; i < setting.unansweredQues.Count; i++)
            {
                counter.Add(i);
            }
            //Debug.Log(numQues);
            for (int i = 0; i < numQues; i++)
            {
                int tempIdx = Random.Range(0, counter.Count);
                int tempQues = counter[tempIdx];
                counter.RemoveAt(tempIdx);
                text += setting.answer[setting.unansweredQues[tempQues]] + "\n";
            }
            paper.GetComponentInChildren<Text>().text = text;

            /*for (int i = 0; i < setting.answer.Length - time; i++)
            {
                text = text + setting.answer[i] + "\n";
            }
            paper.GetComponentInChildren<Text>().text = text;*/
        }
    }

    void Update()
    {
        if (GameObject.Find("LevelSetting") != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                //SceneManager.LoadScene(1);
                LoadSceneHandler.SetActive(true);
            }
        }
    }
}