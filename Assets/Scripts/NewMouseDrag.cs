using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMouseDrag : MonoBehaviour
{

    public GameObject selected;
    public LayerMask layerMask;

    public float whealHeight;
    public float RedThingHeight;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (selected == null && hit.transform.tag == "Wheal" ||
                    selected == null && hit.transform.tag == "RedThing")
                {
                    selected = hit.transform.gameObject;
                }

                if (selected != null && selected.tag == "Wheal" && selected.GetComponent<WhealStatus>()._canMove)
                { 
                    selected.transform.parent = null;
                    selected.GetComponent<Rigidbody>().isKinematic = false;
                    selected.transform.position = new Vector3(hit.point.x, whealHeight, hit.point.z);
                }
                if (selected != null && selected.tag == "RedThing")
                    selected.transform.position = new Vector3(hit.point.x, RedThingHeight, hit.point.z);
            }
        }
        else if(selected != null)
        {
            if (selected.tag == "RedThing")
            {
                selected.GetComponent<Rigidbody>().isKinematic = true;
                selected.GetComponent<Rigidbody>().isKinematic = false;
                selected = null;
            }
            else
            {
                selected = null;
            }
        }

    }
}
