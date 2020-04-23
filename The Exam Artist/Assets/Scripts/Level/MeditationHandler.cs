using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeditationHandler : MonoBehaviour
{
    private float time = 100.0f;
    public GameObject level;
    public GameObject front, right;
    public Text[] frontText, rightText, texts;
    private List<bool> show = new List<bool>();

    void Start()
    {
        frontText = front.GetComponentsInChildren<Text>();
        rightText = right.GetComponentsInChildren<Text>();

        texts = new Text[frontText.Length + rightText.Length];
        frontText.CopyTo(texts, 0);
        rightText.CopyTo(texts, frontText.Length);
        for (int i = 0; i < texts.Length; i++)
        {
            Color c = texts[i].color;
            c.a = 0.0f;
            texts[i].color = c;
            show.Add(false);
        }
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            int index = Random.Range(0, texts.Length);

            if (!show[index])
            {
                FadeTextToFullAlpha(1.0f, texts[index], index);
            }
            else
            {
                FadeTextToZeroAlpha(0.1f, texts[index], index);
            }
        }
        else
        {
            time = 10.0f;
            gameObject.SetActive(false);
            level.SetActive(true);
        }
    }

    public void FadeTextToFullAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
        if(i.color.a >= 1.0f) { show[index] = true; }
    }

    public void FadeTextToZeroAlpha(float t, Text i, int index)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
        if (i.color.a <= 0.0f) { show[index] = false; }
    }
}
