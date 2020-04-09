using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatSheet : MonoBehaviour
{
    private static Vector3 originalPlace;
    private static Vector3 newPlace;
    void Start()
    {
        originalPlace = gameObject.transform.position;
        newPlace = new Vector3(originalPlace.x, originalPlace.y, originalPlace.z - 0.3f);
    }

    public void MoveOut()
    {
        gameObject.transform.position = newPlace;
    }

    public void MoveIn()
    {
        gameObject.transform.position = originalPlace;
    }

    /*private void OnMouseEnter()
    {
        MoveOut();
    }

    private void OnMouseExit()
    {
        MoveIn();
    }*/
}
