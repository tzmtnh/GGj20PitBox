using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float rayLenght;
    public LayerMask layerMask;

    public GameObject wheal;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLenght, layerMask))
            {
                if (hit.transform.tag == "WhealBox")
                {
                    Instantiate(wheal, hit.transform.position, wheal.transform.rotation);
                }

                if(hit.transform.tag == "Jack")
                {
                    hit.transform.localScale += new Vector3(0f,.03f,0f);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLenght, layerMask))
            {

                if (hit.transform.tag == "Jack")
                {
                    hit.transform.localScale += new Vector3(0f, -.03f, 0f);
                }
            }
        }


    }
}
