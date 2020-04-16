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
            gameObject.transform.localPosition = new Vector3(0.514f, 1.2f, 0.848f); 
            gameObject.transform.localEulerAngles = new Vector3(-0.002f, 180.361f, 0.032f);
            gameObject.GetComponent<Throwable>().releaseVelocityStyle = ReleaseStyle.ShortEstimation;
            added = true;
        }
    }
}
