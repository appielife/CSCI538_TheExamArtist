using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    private const float REAL_SECONDS = 24.0f * 60.0f * 60.0f;

    private Transform ClockHourHand, ClockMinuteHand;
    private Text ClockText;
    private float day;

    void Awake()
    {
        ClockHourHand = GameObject.Find("ClockHourHand").transform;
        ClockMinuteHand = GameObject.Find("ClockMinuteHand").transform;
        ClockText = GameObject.Find("ClockText").GetComponent<Text>();
    }

    void Update()
    {
        day += Time.deltaTime / REAL_SECONDS;
        float dayNormalized = day % 1.0f;

        float rotationPerDay = 720.0f;
        ClockHourHand.eulerAngles = new Vector3(0.0f, 0.0f, -dayNormalized * rotationPerDay);

        float hoursPerDay = 24.0f;
        ClockMinuteHand.eulerAngles = new Vector3(0.0f, 0.0f, -dayNormalized * rotationPerDay * hoursPerDay);

        string hour = Mathf.Floor(dayNormalized * 24.0f).ToString("00");
        string minute = (((dayNormalized * hoursPerDay) % 1.0f) * 60.0f).ToString("00");

        ClockText.text = hour + ":" + minute;
    }
}
