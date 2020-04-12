using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{
    private LevelSetting setting;
    void Start()
    {
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
    }

    private void Update()
    {
        if (!setting.onPrepare)
        {
            gameObject.AddComponent<Throwable>();
        }
    }
}
