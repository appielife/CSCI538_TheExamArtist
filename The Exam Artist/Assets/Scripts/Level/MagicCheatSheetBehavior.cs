using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/******************************************************************* 
Script for Washroom Skill:
Go to the washroom for a few seconds. Cost one minute of test time.
A pair to washroom.cs
*******************************************************************/

public class MagicCheatSheetBehavior : MonoBehaviour
{
    [Tooltip("Image for CD")]
    public Image imgCoolDown;
    [Tooltip("Image for existing")]
    public Image imgExist;
    [Tooltip("Text for CD")]
    public Text textCoolDown;

    // Cooldown settings
    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    // Exist settings
    private float existTime = 5.0f, existTimeCounter = 5.0f;
    private bool exist = false, used = false;
    private Text currentHintShown, cheatText;
    static string[] choices = { "A", "B", "C", "D" };

    private GameObject testPaper;
    private TimeFreezeBehavior tf;

    private JArray hintArray;                               // Hint JArray
    private List<JToken> hintJToken = new List<JToken>();   // Hint JToken
    private List<Text> Hints = new List<Text>();            // Hint location text
    private int numHint;                                    // Number of hint

    // Read file before start
    private void Awake()
    {
        // Read JSON directly from a file
        // NOTE: Check out @Application.dataPath in Unity Documents.
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/hints.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            hintArray = (JArray)((JObject)JToken.ReadFrom(reader))["questions"];
        }
        numHint = hintArray.Count;

        for (int i = 0; i < numHint; i++)
        {
            string name = (string)hintArray[i]["name"];
            Hints.Add(GameObject.Find(name).GetComponentInChildren<Text>());
        }
    }

    void Start()
    {
        testPaper = GameObject.FindGameObjectWithTag("MainSelectHandler");

        GameObject cheatsheet = GameObject.Find("CheatSheet");
        cheatText = cheatsheet.transform.Find("Hint").GetComponentInChildren<Text>();
        cheatText.text = "";
        
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();

        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";

        GameObject[] hints = GameObject.FindGameObjectsWithTag("Hint");
        for (int i = 0; i < hints.Length; i++)
        {
            hints[i].GetComponentInChildren<Text>().text = "";
        }
        
        RandomHint();
    }

    void Update()
    {
        // If not time freeze, cooldown remains active
        if (!tf.hold)
        {
            if (existTimeCounter > 0 && exist == true && used == false)
            {
                existTimeCounter -= Time.deltaTime;
                imgExist.fillAmount = 1 - existTimeCounter / existTime;
                textCoolDown.text = ((int)Mathf.Ceil(existTimeCounter)).ToString();
            }
            else if (existTimeCounter <= 0 && exist == true && used == false)
            {
                existTimeCounter = existTime;
                exist = false;
                imgExist.fillAmount = 0.0f;
                cheatText.text = "";
                currentHintShown.text = "";
                used = true;
            }
            else if (coolDownCounter > 0 && used == true)
            {
                coolDownCounter -= Time.deltaTime;
                imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
                textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
            }
            else if (coolDownCounter <= 0 && used == true)
            {
                coolDownCounter = coolDown;
                textCoolDown.text = "";
                imgCoolDown.fillAmount = 0.0f;
                used = false;
            }
        }
    }

    // Function for skill (Main Function)
    public void MagicCheatSheet()
    {
        // If not cooling down and not existing
        if (exist == false && used == false)
        {
            int temp_ques_id = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesId();
            int index = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesNum();
            string hintStr = (string)hintJToken[index]["hints"];
            cheatText.text = hintStr;

            exist = true;
            int tempAnsIdx = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
            int id = (int)hintJToken[index]["id"];

            Hints[id].text = choices[tempAnsIdx];
            currentHintShown = Hints[id];
            if (id == 8)
            {
                GameObject.Find("OutsideAnswer").GetComponentInChildren<femaleoutside>().startAnimation();
                GameObject.Find("OutsideAnswer").GetComponentInChildren<femaleoutside>().enabled = true;
            }
        }
    }

    // Function to know if skill activated
    public bool isTrigger()
    {
        return used == true || exist == true;
    }

    // Function to reset (used in MeditationBehavior.cs)
    public void ResetSkill()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        cheatText.text = "";
        if (currentHintShown)
        {
            currentHintShown.text = "";
        }

        coolDownCounter = coolDown;
        existTimeCounter = existTime;
        exist = false;
        used = false;
    }

    // Function to randomly pick hint to each question
    public void RandomHint()
    {
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        int numQuestion = setting.numQuestion;
        for (int i = 0; i < numQuestion; i++)
        {
            int id = Random.Range(0, hintArray.Count);
            hintJToken.Add(hintArray[id]);
        }
    }
}
