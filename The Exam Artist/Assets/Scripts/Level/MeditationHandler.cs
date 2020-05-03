using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Valve.VR;

/******************************************************************* 
Script for Meditation Scene (Object):
A pair to MeditationBehavior.cs
*******************************************************************/

public class MeditationHandler : MonoBehaviour
{
    [Tooltip("Level Object (Contains Classroom)")]
    public GameObject level;
    [Tooltip("Walls")]
    public GameObject front, right, left, back;
    [Tooltip("Skiils Scipt")]
    public MeditationBehavior meditation;

    private float time = 20.0f, duration = 2.0f;
    private bool play = false, extended = false, isMeditate = false;
    private Text[] frontText, rightText, leftText, backText, texts;
    private GameObject projectile;
    private List<bool> show = new List<bool>();
    private Dictionary<int, Text> showing = new Dictionary<int, Text>(), fading = new Dictionary<int, Text>();
    private List<int> remove = new List<int>();
    private AudioSource[] sound;
    private JObject words_odj;
    private JArray words;
    private LevelSetting setting;
    
    void Awake()
    {
        // Obtain sound
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();

        // Set Text from four sides
        frontText = front.GetComponentsInChildren<Text>();
        rightText = right.GetComponentsInChildren<Text>();
        leftText = left.GetComponentsInChildren<Text>();
        backText = back.GetComponentsInChildren<Text>();

        texts = new Text[frontText.Length + rightText.Length + leftText.Length + backText.Length];
        frontText.CopyTo(texts, 0);
        rightText.CopyTo(texts, frontText.Length);
        leftText.CopyTo(texts, frontText.Length + rightText.Length);
        backText.CopyTo(texts, frontText.Length + rightText.Length + leftText.Length);

        // Read JSON directly from a file
        // NOTE: Check out @Application.dataPath in Unity Documents.
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/Meditation.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            words_odj = (JObject)JToken.ReadFrom(reader);
        }
        words = (JArray)words_odj["words"];

        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
    }

    private void OnEnable()
    {
        isMeditate = true;  // Is meditating

        // Extend text with question texts
        if (!extended)
        {
            for (int i = 0; i < setting.question.getQuesCount(); i++)
            {
                JToken t = setting.question.getQuestionTxt(i);
                words.Add(t);
            }
        }

        sound[2].volume = 0.5f; // Set BGM volume

        // Randomly pick a sentece/word from array
        for (int i = 0; i < texts.Length; i++)
        {
            Color c = texts[i].color;
            c.a = 0.0f;
            texts[i].color = c;
            int index = Random.Range(0, words.Count);
            texts[i].text = (string)words[index];
            show.Add(false);
        }
        projectile = setting.projectile;    // Set projectile
    }

    void Update()
    {
        if (time > 0)
        {
            // Play answer audio when < 5 seconds
            if (time < 5.0f)
            {
                if (!play)
                {
                    StartCoroutine(FadeOut(sound[2], 5.0f));
                    sound[0].PlayOneShot(meditation.meditationAudioClips[meditation.correctAns], 1.0f);
                    play = true;
                }
            }

            time -= Time.deltaTime;

            // Fade in/out text on walls, can improve
            remove = new List<int>();
            foreach (KeyValuePair<int, Text> p in showing)
            {
                FadeTextToFullAlpha(1.0f, p.Value, p.Key);
            }
            for (int i = 0; i < remove.Count; i++)
            {
                showing.Remove(remove[i]);
            }

            remove = new List<int>();
            foreach (KeyValuePair<int, Text> p in fading)
            {
                FadeTextToZeroAlpha(1.0f, p.Value, p.Key);
            }
            for (int i = 0; i < remove.Count; i++)
            {
                fading.Remove(remove[i]);
            }

            int index = Random.Range(0, texts.Length);
            if (!showing.ContainsKey(index) && !fading.ContainsKey(index))
            {
                if (!show[index])
                {
                    FadeTextToFullAlpha(1.0f, texts[index], index);
                }
                else
                {
                    FadeTextToZeroAlpha(0.1f, texts[index], index);
                }
            }
        }
        else
        {
            // If time up, stop BGM and return
            sound[2].Stop();
            time = 15.0f;
            play = false;
            if (SteamVR.active)
            {
                FadeOut();
                Invoke("FadeIn", duration);
            }
            else
            {
                Initiate.Fade("", Color.black, 0.5f);
                Invoke("Change", duration);    
            }
        }
    }

    // Function to fade in text
    public void FadeTextToFullAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
        if (!showing.ContainsKey(index))
        {
            showing.Add(index, i);
        }
        if (i.color.a >= 1.0f)
        {
            show[index] = true;
            remove.Add(index);
        }
    }
    
    // Function to fade out text
    public void FadeTextToZeroAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
        if (!fading.ContainsKey(index))
        {
            fading.Add(index, i);
        }
        if (i.color.a <= 0.0f)
        {
            show[index] = false;
            remove.Add(index);
        }
    }
    
    // Function to fade out audio
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // Function to fade out (SteamVR)
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, duration);
    }

    // Function to fade in (SteamVR)
    private void FadeIn()
    {
        gameObject.SetActive(false);
        level.SetActive(true);
        isMeditate = false;
        projectile.SetActive(true);
        SteamVR_Fade.Start(Color.clear, duration);
    }

    // Function to swap active object
    private void Change()
    {
        gameObject.SetActive(false);
        level.SetActive(true);
        isMeditate = false;
        projectile.SetActive(true);
    }

    // Function to know if in meditation
    public bool inMeditation()
    {
        return isMeditate;
    }

}
