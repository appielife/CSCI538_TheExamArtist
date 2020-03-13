using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    public GameObject player;
    public GameObject transition;
    void Start()
    {
        transition.SetActive(true);
        DontDestroyOnLoad(player);
        //SceneManager.LoadScene(0);
    }
}
