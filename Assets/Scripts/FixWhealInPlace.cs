using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWhealInPlace : MonoBehaviour
{

    public int posWheal;

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wheal")
        {
            other.transform.parent = transform;
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;
            other.GetComponent<WhealStatus>().fixedInPlace = true;
            other.GetComponent<Rigidbody>().isKinematic = true;

        }
    }
}
