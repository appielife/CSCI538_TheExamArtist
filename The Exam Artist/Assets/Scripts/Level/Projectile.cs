using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Projectile : MonoBehaviour
{
    private LevelSetting setting;
    private bool added = false;
    void Start()
    {
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
    }

    private void Update()
    {
        if (!setting.onPrepare && !added)
        {
            gameObject.AddComponent<Throwable>();
            added = true;
        }
    }
}
