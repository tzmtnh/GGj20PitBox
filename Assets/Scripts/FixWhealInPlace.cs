using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWhealInPlace : MonoBehaviour
{
    public bool itsOk = false;

    [SerializeField] private bool _isInverted;
    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (transform.childCount > 0)
        {
            return;
        }

        if (itsOk)
        {
            if (other.tag == "Wheal")
            {
                if (other.GetComponent<WhealStatus>().theyAreNew)
                {
                    other.transform.parent = transform;
                    other.transform.position = transform.position;
                    other.transform.rotation = transform.rotation;
                    if (_isInverted)
                    {
                        other.transform.Rotate(0, 0, 180);
                    }
                    other.GetComponent<WhealStatus>().fixedInPlace = true;
                    other.GetComponent<WhealStatus>()._canMove = false;
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    itsOk = false;
                }

            }
        }
    }
}
