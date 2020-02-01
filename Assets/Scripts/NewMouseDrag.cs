using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMouseDrag : MonoBehaviour
{
	public static NewMouseDrag inst;

    public GameObject selected;
    public LayerMask layerMask;
    public GameObject wheal;
    public float whealHeight = 0.5f;
    public float RedThingHeight = 0.5f;
	public float gasHeight = 0.5f;

	bool _waitForMouseRelease = false;

    [SerializeField] private Transform[] _wheels = new Transform[4];
    private bool[] _wheelsMoved = {false, false, false, false};

	public void WaitForMouseRelease() {
		_waitForMouseRelease = true;
	}

	void Awake() {
		inst = this;
	}

    public void Reset()
    {
        for (int i = 0; i >= 3; i++)
        {
            _wheelsMoved[i] = false;
        }
    }

    void Update()
    {
		bool isLeftMouseButtonPressed = Input.GetMouseButton(0);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (selected == null)
                {
                    if (hit.transform.tag == "WheelMesh")
                    {
                        for (int i=0; i<=3; i++)
                        { 
                            Debug.Log(_wheelsMoved[i]);
                            if (_wheels[i] == hit.transform && _wheelsMoved[i]==false)
                            {
                                _wheelsMoved[i] = true;
                                Debug.Log("Found Mesh");
                                hit.transform.GetComponentInParent<FixWhealInPlace>().itsOk = true;
                                hit.transform.gameObject.SetActive(false);
                                Instantiate(wheal, hit.transform.position, wheal.transform.rotation).GetComponent<WhealStatus>()
                                    .theyAreNew = false;
                            }
                        }
                    }
                }
            }
        }

        if (_waitForMouseRelease == false && isLeftMouseButtonPressed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                if (selected == null)
                {
                    if (	hit.transform.tag == "Wheal" ||
					    hit.transform.tag == "RedThing" ||
						hit.transform.tag == "Gas")
                    {
                        selected = hit.transform.gameObject;
                    }
                    
                }

				if (selected != null) {
					if (selected.tag == "Wheal") {
						selected.transform.position = new Vector3(hit.point.x, whealHeight, hit.point.z);
					} else if (selected.tag == "RedThing") {
						selected.transform.position = new Vector3(hit.point.x, RedThingHeight, hit.point.z);
					} else if (selected.tag == "Gas") {
						selected.transform.position = new Vector3(hit.point.x, gasHeight, hit.point.z);
					}
				}
            }
        }
        else if(selected != null && _waitForMouseRelease == false)
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

		if (_waitForMouseRelease && isLeftMouseButtonPressed == false) {
			_waitForMouseRelease = false;
		}
    }
}
