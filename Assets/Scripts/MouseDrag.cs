using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Rigidbody rb;

    public float lockHeight;
    public bool lockIfNotDrag;

    public bool redThing;
    public bool wheal;
    public bool fuel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (wheal)
        {
            if (lockIfNotDrag && !GetComponent<WhealStatus>().theyAreNew)

            {
                rb.isKinematic = true;
            }

        }
    }

    private void OnMouseDrag()
    {

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + transform.position.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (transform.position.z >= lockHeight)
        {
            if (wheal)
            {
                if (!GetComponent<WhealStatus>().fixedInPlace)
                    transform.position = new Vector3(objPosition.x, lockHeight, objPosition.z);

            }else if (redThing)
            {
                transform.position = new Vector3(objPosition.x, lockHeight, objPosition.z);
            }
        }
        else
        {
            if (wheal)
            {
                if (!GetComponent<WhealStatus>().fixedInPlace)
                    transform.position = objPosition;
            }
            else if (redThing)
            {
                transform.position = objPosition;
            }
        }

        

        rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        if (wheal)
        {
            if (!GetComponent<WhealStatus>().fixedInPlace)
                rb.isKinematic = false;
        }
        else if (redThing)
        {
            rb.isKinematic = false;
        }

        lockIfNotDrag = false;
    }
}
