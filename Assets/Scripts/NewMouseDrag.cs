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

    private Vector3 _oldMouse;
    private Vector3 _mouseSpeed;

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
        for (int i = 0; i <= 3; i++)
        {
            _wheelsMoved[i] = false;
        }
    }

	Vector3 GetPointOnGrid(Ray ray, float gridHeight) {
		Plane grid = new Plane(Vector3.up, new Vector3(0, gridHeight, 0));
		float enter;
		grid.Raycast(ray, out enter);
		return ray.origin + ray.direction * enter;
	}

    void Update()
    {
        _mouseSpeed = _oldMouse - Input.mousePosition;
        _oldMouse = Input.mousePosition;

        if (!Simulation.SimulationInst.IsGameRunning()||!Simulation.SimulationInst.HasGameStarted())
        {
            return;
        }

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
                    else if (hit.transform.tag == "WhealBox")
                    {
                        Instantiate(wheal, hit.transform.position+hit.transform.forward*3, wheal.transform.rotation).GetComponent<WhealStatus>()
                            .theyAreNew = true;
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
                    if (hit.transform.tag == "Wheal" ||
					    hit.transform.tag == "RedThing" ||
						hit.transform.tag == "Gas")
                    {
                        selected = hit.transform.gameObject;
                    }
                    
                }

				if (selected != null) {
					Rigidbody rb = selected.GetComponent<Rigidbody>();

					float gridHeight = 0;
					if (selected.tag == "Wheal") {
						gridHeight = whealHeight;
					} else if (selected.tag == "RedThing") {
						gridHeight = RedThingHeight;
					} else if (selected.tag == "Gas") {
						rb.isKinematic = true;
						gridHeight = gasHeight;
					}

					rb.position = GetPointOnGrid(ray, gridHeight);
				}
			}
        }
        else if(selected != null && _waitForMouseRelease == false)
        {
            if (selected.tag == "RedThing")
            {
                selected.GetComponent<Rigidbody>().isKinematic = false  ;
            } if (selected.tag == "Gas" && ConnectGasHandle.inst.connected == false) {
				selected.GetComponent<Rigidbody>().isKinematic = false;
			}
            selected.GetComponent<Rigidbody>().AddForce(_mouseSpeed*100*Time.deltaTime,ForceMode.Force);

			selected = null;

		}

		if (_waitForMouseRelease && isLeftMouseButtonPressed == false) {
			_waitForMouseRelease = false;
		}
    }
}
