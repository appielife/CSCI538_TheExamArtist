using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultipleChoiceBehavior : MonoBehaviour
{
    public GameObject question;
    public GameObject choiceA;
    public GameObject choiceB;
    public GameObject choiceC;
    public GameObject choiceD;
    public int choice;

    public void reset()
    {
        Button A = choiceA.GetComponent<Button>();
        Button B = choiceB.GetComponent<Button>();
        Button C = choiceC.GetComponent<Button>();
        Button D = choiceD.GetComponent<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
    }

    public void selectA()
    {
        reset();
        Button A = choiceA.GetComponent<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        A.colors = cb;
    }
    public void selectB()
    {
        reset();
        Button B = choiceB.GetComponent<Button>();
        ColorBlock cb = B.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        B.colors = cb;
    }
    public void selectC()
    {
        reset();
        Button C = choiceC.GetComponent<Button>();
        ColorBlock cb = C.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        C.colors = cb;
    }
    public void selectD()
    {
        reset();
        Button D = choiceD.GetComponent<Button>();
        ColorBlock cb = D.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        D.colors = cb;
    }
}
