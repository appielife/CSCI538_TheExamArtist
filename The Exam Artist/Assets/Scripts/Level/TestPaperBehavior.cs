using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Valve.VR;

/************************************************************* 
Script to control player's test behavior:
Switches to next question with correct answer every X seconds.
Gets question from main test paper
*************************************************************/

public class TestPaperBehavior : MonoBehaviour
{
    [Tooltip("Question on test paper")]
    public GameObject questionTextObj;
    [Tooltip("Choices on test paper")]
    public GameObject choiceA, choiceB, choiceC, choiceD;
    [Tooltip("Student images")]
    public Sprite[] studentImage;
    [Tooltip("Load Scene Handler (SteamVR Load Level)")]
    public GameObject LoadSceneHandler;

    [HideInInspector]
    public bool onPrepare = true;               // On prepare
    [HideInInspector]
    public GetQuestion question = new GetQuestion();
    [HideInInspector]
    public List<int> unansweredQues = new List<int>();

    // Different pages
    private GameObject preparePage, testPage, submitPage, initialPage, bribePage, bribeSkillPage;
    private GameObject[] bribeOptions;          // Bribe options on skill page
    private LevelSetting setting;               // Global setting
    private GiftBlindEyesBehavior gbe;          // Skill script
    private MultipleChoiceBehavior[] quesTrack; // Question tracker

    private int tempQuestion = -1;              // Question counter
    private int bribePageCounter = -1;          // Bribe page counter
    private string JSON_file;                   // File name
    private float offset;                       // Talking time
    private bool start = false;                 // Test start

    private Color disabledColor = new Color(0.78f, 0.78f, 0.78f, 1.0f); // Disabled button color
    private Color bribeColor = new Color(0.98f, 0.812f, 0.016f, 1.0f);  // Bribe button color

    void Start()
    {
        gbe = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();

        GameObject testPaper = GameObject.FindGameObjectWithTag("MainTestPaper");
        testPage = testPaper.transform.Find("TestPage").gameObject;
        submitPage = testPaper.transform.Find("SubmitPage").gameObject;
        initialPage = testPaper.transform.Find("InitialPage").gameObject;
        preparePage = testPaper.transform.Find("PreparePage").gameObject;
        bribePage = testPaper.transform.Find("BribePage").gameObject;
        bribeSkillPage = testPaper.transform.Find("BribeSkillPage").gameObject;

        // Start with prepare page
        preparePage.SetActive(true);
        initialPage.SetActive(false);
        testPage.SetActive(false);
        submitPage.SetActive(false);
        bribePage.SetActive(false);
        bribeSkillPage.SetActive(false);

        // Randomly pick a subject
        string[] files = { "World", "Chemistry", "Math", "Biology", "Computer", "History", "Music", "BioChemistry", "Sports", "Geography" };
        int index = Random.Range(0, files.Length);
        string filename = "questions-" + files[index] + ".json";
        JSON_file = filename;

        // Set subject on test papers and black board
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        setting.setSubject(files[index]);

        // Set onprepare
        onPrepare = setting.onPrepare;
        if (!onPrepare)
        {
            // Show initial page if onprepare not set
            initialPage.SetActive(true);
            preparePage.SetActive(false);
        }

        // Set talking time
        offset = setting.offset;
        
        // Set number of question first, then read questions from JSON
        question.setNumQuestion(setting.numQuestion);
        question.readQuestionFromJson(JSON_file);
        // Multiple Choice Behavior for EACH question
        quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];

        // Get all questions loaded to get answers for level setting
        for (int i = 0; i < question.getQuesCount(); i++)
        {
            next();
        }
        // Set unanswered questions
        for (int i = 0; i < setting.numQuestion; i++)
        {
            unansweredQues.Add(i);
        }
    }

    void Update()
    {
        // If ready for test
        if (!onPrepare)
        {
            // If not finish talking
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (!start)
                {
                    // If haven't swith page
                    initialPage.SetActive(false);
                    testPage.SetActive(true);
                    next();
                    start = true;
                }
            }
        }
    }

    // Function to get current question number
    public int getCurrentQuesNum()
    {
        return tempQuestion;
    }

    // Function to get current question ID
    public int getCurrentQuesId()
    {
        return int.Parse(question.getQuestionId(tempQuestion));
    }
    
    // Function to get current question answer
    public int getCurrentQuesAns()
    {
        return quesTrack[tempQuestion].correctAns;
    }
    
    // Function to next question
    public void next()
    {
        reset();
        // Set question counter
        if (tempQuestion < question.getQuesCount() - 1)
        {
            tempQuestion += 1;
        }
        else
        {
            tempQuestion = 0;
        }
        // Set question
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getNextQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        // Set tag of selected choice if answered before
        if (quesTrack[tempQuestion].choice > -1)
        {
            setSelectedTag(quesTrack[tempQuestion].choice);
        }
    }

    // Function to previous question
    public void previous()
    {
        reset();
        // Set question counter
        if (tempQuestion > 0)
        {
            tempQuestion -= 1;
        }
        else
        {
            tempQuestion = question.getQuesCount() - 1;
        }
        // Set question
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getPrevQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        // Set tag of selected choice if answered before
        if (quesTrack[tempQuestion].choice > -1)
        {
            setSelectedTag(quesTrack[tempQuestion].choice);
        }
    }

    // Function starting test (finish preparing)
    public void startTest()
    {
        // Switch page to initail page
        preparePage.SetActive(false);
        initialPage.SetActive(true);
        onPrepare = false;
        setting.setOnPrepare(false);
        Text subjectText = GameObject.FindGameObjectWithTag("MainSubject").GetComponent<Text>();
        subjectText.text = setting.subject;
    }

    // Function to open bribe page (in prepare)
    public void openBribePage()
    {
        // Switch to bribe page
        preparePage.SetActive(false);
        bribePage.SetActive(true);
        if (bribeOptions == null || bribeOptions.Length == 0)
        {
            // Find option locations on page
            bribeOptions = GameObject.FindGameObjectsWithTag("BribeOption");
        }
        bribePageNext();
    }

    // Function to next page in bribe page
    public void bribePageNext()
    {
        // Update bribe page counter
        if (bribePageCounter + 1 < Mathf.Ceil((float)studentImage.Length / bribeOptions.Length))
        {
            bribePageCounter += 1;
        }
        else
        {
            bribePageCounter = 0;
        }
        showStudentImage(); // Update student images
    }

    // Function to previous page in bribe page
    public void bribePagePrev()
    {
        // Update bribe page counter
        if (bribePageCounter > 0)
        {
            bribePageCounter -= 1;
        }
        else
        {
            bribePageCounter = (int)Mathf.Ceil((float)studentImage.Length / bribeOptions.Length) - 1;
        }
        showStudentImage(); // Update student images
    }

    // Function to show student image(s) on bribe page
    private void showStudentImage()
    {
        for (int i = 0; i < bribeOptions.Length; i++)
        {
            // Show bribe options for current page
            if (bribePageCounter * bribeOptions.Length + i < studentImage.Length)
            {
                // Obtain image and change image
                Image img = bribeOptions[i].GetComponent<Image>();
                img.sprite = studentImage[bribePageCounter * bribeOptions.Length + i];
                // Activate current option
                bribeOptions[i].transform.parent.parent.gameObject.SetActive(true);

                // If option bribe selected before, show cancel button; else, show bribe button
                if (gbe.bribeList.Contains(img.sprite))
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text = "Cancel";
                }
                else
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text = "Bribe";
                }
                // If already chosen 3 student, cannot choose more students
                if (gbe.bribeList.Count == 3)
                {
                    if (bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
                    {
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = false;
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = disabledColor;
                    }
                    else
                    {
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                        bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                    }
                }
                else
                {
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                    bribeOptions[i].transform.parent.parent.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                }
            }
            else
            {
                // Hide extra option
                bribeOptions[i].transform.parent.parent.gameObject.SetActive(false);
            }

        }
    }

    // Function to show bribe skill page (Page for Bribe Skill)
    public void showBribeSkillPage()
    {
        // If bribe skill not cooling down
        if (!gbe.isCoolDown())
        {
            // Swtich page
            testPage.SetActive(false);
            bribeSkillPage.SetActive(true);
            // Show bribed options
            for (int i = 0; i < gbe.bribeList.Count; i++)
            {
                bribeSkillPage.transform.GetChild(i).gameObject.SetActive(true);
                bribeSkillPage.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = gbe.bribeList[i];
            }
        }
    }

    // Function for onclick bribe/cancel button on bribe page
    public void bribeStudent(GameObject target)
    {
        if (target.transform.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
        {
            target.transform.GetChild(1).GetComponentInChildren<Text>().text = "Cancel";
            gbe.bribeList.Add(target.transform.GetChild(0).GetComponentInChildren<Image>().sprite);
        }
        else
        {
            target.transform.GetChild(1).GetComponentInChildren<Text>().text = "Bribe";
            gbe.bribeList.Remove(target.transform.GetChild(0).GetComponentInChildren<Image>().sprite);
        }
        GameObject options = target.transform.parent.gameObject;
        for (int i = 0; i < bribeOptions.Length; i++)
        {
            GameObject temp = options.transform.GetChild(i).gameObject;
            if (temp.transform.GetChild(1).GetComponentInChildren<Text>().text == "Bribe")
            {
                if (gbe.bribeList.Count == 3)
                {
                    temp.transform.GetChild(1).GetComponentInChildren<Button>().enabled = false;
                    temp.transform.GetChild(1).GetComponentInChildren<Image>().color = disabledColor;
                }
                else
                {
                    temp.transform.GetChild(1).GetComponentInChildren<Button>().enabled = true;
                    temp.transform.GetChild(1).GetComponentInChildren<Image>().color = bribeColor;
                }
            }
        }
    }

    // Function to choose bribed student (Main Function for Bribe Skill)
    public void ChooseBribee(GameObject t)
    {
        gbe.ChooseBribee(t);
        backToTest(); // Change page
    }

    // Funtion for back button on bribe page
    public void backToPreparePage()
    {
        bribePage.SetActive(false);
        preparePage.SetActive(true);
        bribePageCounter = -1;
    }

    // Function to see if bribe skill page is shown
    public bool isBribeSkillActive()
    {
        return bribeSkillPage.activeSelf;
    }

    // Function to see if bribe page is shown
    public bool isBribeActive()
    {
        return bribePage.activeSelf;
    }

    // Function for submit button
    public void submit()
    {
        Button submit = testPage.GetComponentsInChildren<Button>()[6];
        ColorBlock cb = submit.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        submit.colors = cb;
        // Change page
        testPage.SetActive(false);
        submitPage.SetActive(true);
    }

    // Funtion for back button on bribe skill page and submit page
    public void backToTest()
    {
        if (submitPage.activeSelf)
        {
            Button no = submitPage.GetComponentsInChildren<Button>()[1];
            ColorBlock cb = no.colors;
            cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            no.colors = cb;
            submitPage.SetActive(false);
        }
        if (bribeSkillPage.activeSelf)
        {
            bribeSkillPage.SetActive(false);
        }
        testPage.SetActive(true);
    }

    // Funtion to get questions
    public GetQuestion getQuestion()
    {
        return question;
    }

    // Funtion to get unanswered questions
    public List<int> getUnansweredQues()
    {
        return unansweredQues;
    }
    
    // Funtion to get all answers (Used in LevelSetting.cs)
    public string[] getAllAnswer()
    {
        int size = question.getQuesCount();
        string[] answer = new string[size];
        char[] abcd = { 'A', 'B', 'C', 'D' };
        for (int i = 0; i < size; i++)
        {
            char ans = abcd[question.getQuestionCorrectAns(i)];
            answer[i] = (i + 1).ToString() + ": " + ans;
        }
        return answer;
    }

    // Funtion when got caught. Switch to gameover page
    public void gameOver()
    {
        testPage.SetActive(true);
        testPage.transform.Find("question").gameObject.SetActive(false);
        testPage.transform.Find("choices").gameObject.SetActive(false);
        testPage.transform.Find("PageNavigator").gameObject.SetActive(false);
        testPage.transform.Find("SubmitTool").gameObject.SetActive(false);
        testPage.transform.Find("GameOverText").gameObject.SetActive(true);
        submitPage.SetActive(false);
        bribeSkillPage.SetActive(false);
    }

    // Funtion to write answer to JSON file
    public void writeAnsToJson()
    {
        char[] abcd = { 'A', 'B', 'C', 'D' };
        using (StreamWriter file = File.CreateText(@Application.dataPath + "/GameData/answers.json"))

        using (JsonWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            writer.WritePropertyName("Answers");
            writer.WriteStartArray();
            for (int i = 0; i < question.getQuesCount(); i++)
            {
                writer.WriteStartObject();

                // Write ID 
                writer.WritePropertyName("id");
                writer.WriteValue(question.getQuestionId(i));

                // Write question text
                writer.WritePropertyName("question_txt");
                writer.WriteValue(question.getQuestionTxt(i));

                // Write player answer
                writer.WritePropertyName("YourAns");
                if (quesTrack[i] == null || quesTrack[i].choice == -1) writer.WriteValue("NA");
                else writer.WriteValue(abcd[quesTrack[i].choice] + ". " + quesTrack[i].choiceContent);

                // Write correct answer
                writer.WritePropertyName("MyAns");
                writer.WriteValue(abcd[question.getQuestionCorrectAns(i)] + ". " + question.getQuestionCorrectAnsContext(i));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        // Fade to GameOver scene
        if (SteamVR.active)
        {
            LoadSceneHandler.SetActive(true);
        }
        else
        {
            Initiate.Fade("GameOver", Color.black, 0.5f);
        }
    }

    // Function to set selected choice's tag
    public void setSelectedTag(int i)
    {
        Button btn = choiceA.GetComponentInChildren<Button>();
        switch (i)
        {
            case 1:
                btn = choiceB.GetComponentInChildren<Button>();
                break;
            case 2:
                btn = choiceC.GetComponentInChildren<Button>();
                break;
            case 3:
                btn = choiceD.GetComponentInChildren<Button>();
                break;

        }
        btn.tag = "MainChoiceSelected";
    }

    // Function to reset page choices
    public void reset()
    {
        Button A = choiceA.GetComponentInChildren<Button>();
        Button B = choiceB.GetComponentInChildren<Button>();
        Button C = choiceC.GetComponentInChildren<Button>();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
        A.tag = "MainChoice";
        B.tag = "MainChoice";
        C.tag = "MainChoice";
        D.tag = "MainChoice";
    }

    // Function to select A
    public void selectA()
    {
        reset();
        choiceA.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceA, 0);
        unansweredQues.Remove(tempQuestion);
    }

    // Function to select B
    public void selectB()
    {
        reset();
        choiceB.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceB, 1);
        unansweredQues.Remove(tempQuestion);
    }

    // Function to select C
    public void selectC()
    {
        reset();
        choiceC.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceC, 2);
        unansweredQues.Remove(tempQuestion);
    }

    // Function to select D
    public void selectD()
    {
        reset();
        choiceD.GetComponentInChildren<Button>().tag = "MainChoiceSelected";
        quesTrack[tempQuestion].select(choiceD, 3);
        unansweredQues.Remove(tempQuestion);
    }

    // Function to know if test started
    public bool isStart()
    {
        return start;
    }
}
