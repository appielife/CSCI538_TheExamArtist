using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    public GameObject player;
    public void Awake()
    {
        DontDestroyOnLoad(player);
        SceneManager.LoadScene(1);
    }
}
