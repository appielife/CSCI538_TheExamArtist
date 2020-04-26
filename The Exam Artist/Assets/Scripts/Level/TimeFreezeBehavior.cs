using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TimeFreezeBehavior : MonoBehaviour
{
    public Image imgCoolDown, imgExist;
    public Text textCoolDown;
    private GameObject teacherCharacter;
    private GameObject[] studentCharacters;
    private float teacherSpeed;
    
    private float coolDown = 5.0f, coolDownCounter = 5.0f;
    private float existTime = 10.0f, existTimeCounter = 10.0f;
    private bool exist = false, used = false;

    private AudioSource[] sound;
    public AudioClip timeFreezeAudioClip;
    // Start is called before the first frame update
    void Start()
    {
        imgCoolDown.fillAmount = 0.0f;
        imgExist.fillAmount = 0.0f;
        textCoolDown.text = "";

        teacherCharacter = GameObject.FindGameObjectWithTag("TeacherAction");
        teacherSpeed = teacherCharacter.GetComponent<NavMeshAgent>().speed;
        studentCharacters = GameObject.FindGameObjectsWithTag("StudentCharacter");
        sound = GameObject.FindGameObjectWithTag("Player").GetComponents<AudioSource>();
    }

    void Update()
    {
        if (existTimeCounter > 0 && exist == true && used == false)
        {
            existTimeCounter -= Time.deltaTime;
            imgExist.fillAmount = 1 - existTimeCounter / existTime;
            textCoolDown.text = ((int)Mathf.Ceil(existTimeCounter)).ToString();
        }
        else if (existTimeCounter <= 0 && exist == true && used == false)
        {
            existTimeCounter = existTime;
            exist = false;
            imgExist.fillAmount = 0.0f;
            //tableHint.text = "";
            UnfreezeCharacters();         
            used = true;
        }
        else if (coolDownCounter > 0 && used == true)
        {
            coolDownCounter -= Time.deltaTime;
            imgCoolDown.fillAmount = 1 - coolDownCounter / coolDown;
            textCoolDown.text = ((int)Mathf.Ceil(coolDownCounter)).ToString();
        }
        else if (coolDownCounter <= 0 && used == true)
        {
            coolDownCounter = coolDown;
            textCoolDown.text = "";
            imgCoolDown.fillAmount = 0.0f;
            used = false;
        }
    }

    public void TimeFreeze()
    {
        if (exist == false && used == false)
        {
            sound[0].PlayOneShot(timeFreezeAudioClip, 1.5f);
            teacherCharacter.GetComponent<Animator>().enabled = false;
            teacherCharacter.GetComponent<NavMeshAgent>().speed = 0;
            teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 0;
            for (int i = 0; i < studentCharacters.Length; i++)
            {
                studentCharacters[i].GetComponent<Animator>().enabled = false;
            }
            exist = true;
        }
        else
        {
            Debug.Log("Your skill need to be cooled down");
        }
    }

    private void UnfreezeCharacters()
    {
        teacherCharacter.GetComponent<Animator>().enabled = true;
        teacherCharacter.GetComponent<NavMeshAgent>().speed = teacherSpeed;
        teacherCharacter.GetComponent<NavMeshAgent>().angularSpeed = 120;
        for (int i = 0; i < studentCharacters.Length; i++)
        {
            studentCharacters[i].GetComponent<Animator>().enabled = true;
        }
    }

    public bool isExisting()
    {
        return exist;
    }
}
