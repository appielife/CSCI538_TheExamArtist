using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllegalMoveHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("GameObject Hit: " + col.gameObject.name);
    }
}
