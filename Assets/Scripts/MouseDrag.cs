using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Rigidbody rb;

    public float lockHeight;
    public bool lockIfNotDrag;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (lockIfNotDrag && !GetComponent<WhealStatus>().theyAreNew)
        {
            rb.isKinematic = true;
        }

    }

    private void OnMouseDrag()
    {

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z + transform.position.z);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (transform.position.z >= lockHeight)
        {
            if (!GetComponent<WhealStatus>().fixedInPlace)
                transform.position = new Vector3(objPosition.x, lockHeight, objPosition.z);
        }
        else
        {
            if (!GetComponent<WhealStatus>().fixedInPlace)
                transform.position = objPosition;
        }

        

        rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
       if(!GetComponent<WhealStatus>().fixedInPlace)
        rb.isKinematic = false;
 
        lockIfNotDrag = false;
    }
}
