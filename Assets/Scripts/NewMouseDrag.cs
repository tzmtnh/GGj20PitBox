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
                if (selected == null)
                { 
                    if(	hit.transform.tag == "Wheal" ||
					    hit.transform.tag == "RedThing" ||
						hit.transform.tag == "Gas")
                    {
                        selected = hit.transform.gameObject;
                    }
                    else if (hit.transform.tag=="WheelMesh")
                    {

                    }
                }


                if (selected != null && selected.tag == "Wheal")
                { 
                    selected.transform.position = new Vector3(hit.point.x, whealHeight, hit.point.z);
                }
                if (selected != null && (selected.tag == "RedThing" ||  selected.tag == "Gas"))
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
