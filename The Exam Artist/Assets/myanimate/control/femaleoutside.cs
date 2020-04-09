using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class femaleoutside : MonoBehaviour { 

    public Animator ani;
    public Random ran = new Random();
    float m_Speed;
    
    //private float timeLeft = 15.0f;
// Start is called before the first frame update
    void Start()
    {
        m_Speed = 5.0f;
        ani.SetInteger("animation_int", 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(Input.GetKeyDown(KeyCode.Alpha0)){
            
            if(transform.position.z > -10 ){
                
                transform.Translate(Vector3.right * Time.deltaTime * m_Speed,Space.Self);

            }
            else{
            var v = transform.localPosition;
            v.z = 30;
            transform.localPosition = v;
            }
            
        //}

        
    }

}
