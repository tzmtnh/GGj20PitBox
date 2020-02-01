using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float rayLenght;
    public LayerMask layerMask;

    public GameObject wheal;

    public float jackMax;
    public float jackMin;

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

                if(hit.transform.tag == "Jack" && hit.transform.localScale.y < jackMax)
                {
                    hit.transform.localScale += new Vector3(0f,.1f,0f);
                }
            }

            Debug.Log(hit.point);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLenght, layerMask))
            {

                if (hit.transform.tag == "Jack" && hit.transform.localScale.y > jackMin)
                {
                    hit.transform.localScale += new Vector3(0f, -.1f, 0f);
                }
            }
        }


    }
}
