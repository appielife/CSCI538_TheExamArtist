using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json.Linq;

/*public class ReadOnlyAttribute : PropertyAttribute { }

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
}*/

public class LevelSetting : MonoBehaviour
{
    public bool start = false;
    public float offset, initialTime = 300.0f;
    public float timeLeft = -1.0f;
    public string[] answer;
    public bool washroomed = false;
    public float washroomDuration = 10.0f;
    public List<int> unansweredQues;
    public GetQuestion question;
    public MultipleChoiceBehavior[] quesTrack;
    public int[] scoreTrack;
    public Settings setting;
    public string subject;
    public List<Vector3> positions = new List<Vector3>();
    public int numQuestion = 5;
    public bool randomseats = false;
    public bool randomed = false;
    public List<JToken> hints = new List<JToken>();
    public bool onPrepare = true;
    public List<Sprite> bribeList = new List<Sprite>();

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
        unansweredQues = test.setUnansweredQues();
    }

    public void setHint()
    {
        MagicCheatSheetBehavior test = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        hints = test.setHints();
    }

    public void resetTemp()
    {
        //Debug.Log("reset");
        start = false;
        question = null;
        quesTrack = null;
        scoreTrack = null;
        unansweredQues = null;
        washroomed = false;
        timeLeft = -1.0f;
        offset = 15.0f;
        onPrepare = true;
        bribeList = new List<Sprite>();
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

    public void setOnPrepare(bool b)
    {
        onPrepare = b;
    }

    public void setPositions(List<Vector3> positions)
    {
        this.positions = positions;
    }

}
