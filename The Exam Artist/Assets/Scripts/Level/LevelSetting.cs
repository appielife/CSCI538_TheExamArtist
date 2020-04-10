using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

public class LevelSetting : MonoBehaviour
{
    [ReadOnly] public bool start = false;
    public float offset, initialTime = 300.0f;
    [ReadOnly] public float timeLeft = -1.0f;
    [ReadOnly] public string[] answer;
    [ReadOnly] public bool washroomed = false;
    [ReadOnly] public GetQuestion question;
    [ReadOnly] public MultipleChoiceBehavior[] quesTrack;
    [ReadOnly] public int[] scoreTrack;
    [ReadOnly] public Settings setting;
    [ReadOnly] public string subject;
    public int numQuestion = 5;
    public bool randomseats = false;

    private void Start()
    {
        timeLeft = -1.0f;
        question = null;
        DontDestroyOnLoad(GameObject.Find("LevelSetting"));
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
    }

    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (!start)
            {
                answer = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>().getAllAnswer();
                //setQuestion();
                start = true;
            }
        }
    }

    public void setTime()
    {
        timeLeft = GameObject.Find("Timer").GetComponent<Timer>().timeLeft;
    }

    public void setWashroom()
    {
        washroomed = true;
    }

    public void setQuestion()
    {
        TestPaperBehavior test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        question = test.setQuestion();
        quesTrack = test.setQuesTrack();
        scoreTrack = test.setScoreTrack();
    }

    public void resetTemp()
    {
        Debug.Log("reset");
        start = false;
        question = null;
        quesTrack = null;
        scoreTrack = null;
        washroomed = false;
        timeLeft = -1.0f;
        offset = 15.0f;
    }

    public void setSubject(string s)
    {
        subject = s;
        GameObject.Find("Subject").GetComponent<Text>().text = s.ToUpper() + " QUIZ !";
        GameObject[] test = GameObject.FindGameObjectsWithTag("Subject");
        for(int i = 0; i< test.Length; i++)
        {
            test[i].GetComponent<Text>().text = s;
        }
    }

}
