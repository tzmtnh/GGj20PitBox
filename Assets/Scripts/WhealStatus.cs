using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhealStatus : MonoBehaviour
{

    public bool theyAreNew;
    public bool fixedInPlace;
    public bool _canMove=true;

    public void TurnKinematicTrue()
    {

        GetComponent<Rigidbody>().isKinematic = false;

    }

    public void Spawn()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        theyAreNew = true;
        fixedInPlace = false;
        _canMove = true;
    }



}
