using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class MagicCheatSheetBehavior : MonoBehaviour
{
    private JArray hintArray;
    public GameObject testPaper, hintObj;
    public Image imgCoolDown, imgExist;
    public Text textCoolDown;
    
    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 5.0f, existTimeCounter = 5.0f;
    private bool exist = false,  used = false;
    private Text tempHintShow;
    private Text cheatText;
    static string[] choices = { "A", "B", "C", "D" };

    private void loadResources()
    {
        GameObject table = GameObject.Find("PlayerTable");
        GameObject SkillsOverlay = table.transform.Find("SkillsOverlay").gameObject;
        GameObject SkillCoolDown = SkillsOverlay.transform.Find("SkillCoolDown").gameObject;
        GameObject skill = SkillCoolDown.transform.Find("MagicCheatSheet").gameObject;
        GameObject resources = skill.transform.Find("Image").gameObject;

        imgCoolDown = resources.transform.Find("CDImg").gameObject.GetComponent<Image>();
        imgExist = resources.transform.Find("exsiting").gameObject.GetComponent<Image>();
        textCoolDown = resources.transform.Find("CDText").gameObject.GetComponent<Text>();

        GameObject cheatsheet = GameObject.Find("CheatSheet");
        cheatText = cheatsheet.transform.Find("Hint").GetComponentInChildren<Text>();
    }
    void Awake()
    {
        loadResources();

        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        hintObj.GetComponentInChildren<Text>().text = "";
        cheatText.text = "";
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/hints.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            hintArray = (JArray)((JObject)JToken.ReadFrom(reader))["questions"];
            //Debug.Log(hintArray);
        }

        JArray temp = new JArray();
        int j = 0;
        for (int i = 0; i < 31; i++)
        {
            temp.Add(hintArray[j]);
            temp[i]["id"] = i.ToString();
            if(j == 4) { j = 0; }
            else { j++; }
        }
        hintArray = temp;
        //Debug.Log(hintArray);
    }
    void Update()
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
            hintObj.GetComponentsInChildren<Text>()[0].text = "";
            cheatText.text = "";
            tempHintShow.text = "";
            used = true; 
        }
        else if (coolDownCounter > 0 && used == true)
        {
            coolDownCounter -= Time.deltaTime;
            imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
            textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
            //Debug.Log(coolDownCounter[0]);
        }
        else if (coolDownCounter <= 0 && used == true)
        {
            coolDownCounter = coolDown;
            textCoolDown.text = "";
            imgCoolDown.fillAmount = 0.0f;
            used = false;
        }
    }
    public void MagicCheatSheet()
    {
        if (exist == false && used == false)
        {
            int temp_ques_id = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesId();
            //Debug.Log(temp_ques_id);
            //Debug.Log((JObject)hintArray[temp_ques_id]);
            string hintStr = (string)((JArray)((JObject)hintArray[temp_ques_id])["hints"])[0];
            hintObj.GetComponentsInChildren<Text>()[0].text = (testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesNum() + 1).ToString() + ". " + hintStr;
            cheatText.text = (testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesNum() + 1).ToString() + ". " +  hintStr;
            exist = true;
            int tempAnsIdx = testPaper.GetComponent<TestPaperBehavior>().getCurrentQuesAns();
            switch (hintStr){
                case "Look at the ceiling":
                    hintObj.GetComponentsInChildren<Text>()[1].text = choices[tempAnsIdx];
                    tempHintShow = hintObj.GetComponentsInChildren<Text>()[1];
                    break;
                case "Look at the wall to the right":
                    hintObj.GetComponentsInChildren<Text>()[2].text = choices[tempAnsIdx];
                    tempHintShow = hintObj.GetComponentsInChildren<Text>()[2];
                    break;
                case "Look at the lower right of the board":
                    hintObj.GetComponentsInChildren<Text>()[3].text = choices[tempAnsIdx];
                    tempHintShow = hintObj.GetComponentsInChildren<Text>()[3];
                    break;
                case "Look at the back of the teacher":
                    GameObject.FindGameObjectsWithTag("Hint")[0].GetComponentInChildren<Text>().text = choices[tempAnsIdx];
                    tempHintShow = GameObject.FindGameObjectsWithTag("Hint")[0].GetComponentInChildren<Text>();
                    break;
                case "Look in your desk drawer":
                    hintObj.GetComponentsInChildren<Text>()[4].text = choices[tempAnsIdx];
                    tempHintShow = hintObj.GetComponentsInChildren<Text>()[4];
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Your skill need to be cooled down");
        }
    }

    public bool isTrigger()
    {
        return used == true || exist == true;
    }

    public void ResetSkill()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";
        hintObj.GetComponentInChildren<Text>().text = "";
        if (tempHintShow)
        {
            tempHintShow.text = "";
        }

        coolDownCounter = coolDown;
        existTimeCounter = existTime;
        exist = false;
        used = false;
    }
}
