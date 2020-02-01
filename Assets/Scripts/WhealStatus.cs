using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhealStatus : MonoBehaviour
{

    public bool theyAreNew;
    public bool fixedInPlace;

    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }



}
