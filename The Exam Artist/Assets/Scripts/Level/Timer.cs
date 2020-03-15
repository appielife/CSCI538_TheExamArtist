using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float timeLeft;
    private bool timesUp = false;
    private TestPaperBehavior test;

    void Start()
    {
        GameObject player = GameObject.Find("TestAndScore");
        test = player.transform.Find("SelectHandler").gameObject.GetComponent<TestPaperBehavior>();
        timeLeft += 1;
    }
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            string minutes = ((int)timeLeft / 60).ToString();
            string seconds = ((int)timeLeft % 60).ToString();
            if ((int)timeLeft / 60 < 10) { minutes = "0" + minutes; }
            if ((int)timeLeft % 60 < 10) { seconds = "0" + seconds; }
            timerText.text = minutes + ":" + seconds;
        }
        else if (timeLeft < 0 && !timesUp)
        {
            test.writeAnsToJson();
            timesUp = true;
        }
    }

}
