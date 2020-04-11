using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeat : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private List<string> names = new List<string>();
    static System.Random random = new System.Random();

    void Start()
    {
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        if (setting.randomseats)
        {
            getPosition();
            for (int i = 0; i < 12; i++)
            {
                positions.Add(GameObject.Find(names[i]).transform.position);
                Debug.Log(positions[i]);
            }
            randomize();
            for (int i = 0; i < 12; i++)
            {
                GameObject.Find(names[i]).transform.position = positions[i];
            }
        }
    }

    void randomize()
    {
        int n = positions.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + random.Next(n - i);
            Vector3 t = positions[r];
            positions[r] = positions[i];
            positions[i] = t;
        }
    }

    void getPosition()
    {
        string current = "";
        for (int i = 0; i < 12; i++)
        {
            if (i < 5)
            {
                current = "Student" + (i + 1).ToString();
            }
            else if (i == 5)
            {
                current = "PlayerStudent";
            }
            else
            {
                current = "Student" + i.ToString();
            }
            names.Add(current);
        }
    }
}
