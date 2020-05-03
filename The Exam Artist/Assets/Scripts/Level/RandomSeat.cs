using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************** 
Script for randomize seats
**************************/

public class RandomSeat : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();  // Student positions
    private List<string> names = new List<string>();        // Student names
    static System.Random random = new System.Random();

    void Start()
    {
        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        // If randeomseats is set
        if (setting.randomseats)
        {
            getPosition(); // Obtain student object name
            for (int i = 0; i < 12; i++)
            {
                positions.Add(GameObject.Find(names[i]).transform.position); // Add student position
            }
            randomize(); // Randomize list
            for (int i = 0; i < 12; i++)
            {
                GameObject.Find(names[i]).transform.position = positions[i]; // Set position
            }
        }
    }

    // Function to randomize list/array
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
    
    // Function to get student object name
    void getPosition()
    {
        GameObject[] students = GameObject.FindGameObjectsWithTag("Student");
        GameObject player = GameObject.FindGameObjectWithTag("MainPlayerPosition");

        for (int i = 0; i < students.Length; i++)
        {
            names.Add(students[i].name);
        }
        names.Add(player.name);
    }
}
