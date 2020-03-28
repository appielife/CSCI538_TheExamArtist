using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class male1 : MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
    
    //private float timeLeft = 15.0f;
// Start is called before the first frame update
    void Start()
    {
        ani.SetInteger("animation_int", 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(transform.position.z < 25){
            transform.Translate(Vector3.forward * Time.deltaTime,Space.Self);
            // int index = Random.Range(3, 7);
            // if(index == 5){
            //     StartCoroutine(Pausing());
            // }
        }
        else{
            var v = transform.localPosition;
            v.z = -11;
            transform.localPosition = v;
        }

        

 
    }
    IEnumerator Pausing(){
        
            ani.SetInteger("animation_int", 2);
            yield return new WaitForSeconds(3);
            ani.SetInteger("animation_int", 1);

            //Debug.Log("Before Waiting 3 seconds");
        }
}
