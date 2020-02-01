using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWhealInPlace : MonoBehaviour
{
    public bool itsOk;
    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (!itsOk)
        {
            if (other.tag == "Wheal")
            {
                if (other.GetComponent<WhealStatus>().theyAreNew)
                {
                    other.transform.parent = transform;
                    other.transform.position = transform.position;
                    other.transform.rotation = transform.rotation;
                    other.GetComponent<WhealStatus>().fixedInPlace = true;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    itsOk = true;
                }

            }
        }
    }
}
