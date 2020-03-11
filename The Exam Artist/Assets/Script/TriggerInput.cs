using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TriggerInput : MonoBehaviour
{
    public SteamVR_Action_Boolean Ypressed;
    public SteamVR_Input_Sources handType;
    public GameObject skills;
    // Update is called once per frame
    void Start()
    {
        Ypressed.AddOnStateDownListener(TriggerDown, handType);
        Ypressed.AddOnStateUpListener(TriggerUp, handType);
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        skills.GetComponent<SkillsBehavior>().GodOfWashroom();
    }

}
