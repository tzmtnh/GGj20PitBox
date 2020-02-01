using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhealBox : MonoBehaviour
{
    public float rayLenght;
    public LayerMask layerMask;

    public GameObject wheal;

    // Update is called once per frame
    void Update()
    {
        if (!Simulation.SimulationInst.IsGameRunning())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, rayLenght, layerMask))
            {
                if(hit.transform.tag == "WhealBox")
                {
                    Instantiate(wheal, hit.transform.position, wheal.transform.rotation).GetComponent<WhealStatus>().theyAreNew=true;
                }
            }
        }

        
    }
}
