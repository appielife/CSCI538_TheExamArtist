using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Washroom : MonoBehaviour
{
    public LevelSetting setting;
    private float timer = 10.0f;
    private GameObject paper;
    void Start()
    {
        if (GameObject.Find("LevelSetting") != null)
        {
            setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
            paper = GameObject.Find("Paper");
            int time = (int)(setting.timeLeft + 120.0f) / 60;
            string text = "";

            for (int i = 0; i < setting.answer.Length - time; i++)
            {
                text = text + setting.answer[i] + "\n";
            }
            paper.GetComponentInChildren<Text>().text = text;
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
                SceneManager.LoadScene(1);
            }
        }
    }
}