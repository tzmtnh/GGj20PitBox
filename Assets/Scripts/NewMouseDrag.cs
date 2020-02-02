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

    private Vector3 _oldRag;
    private Vector3 _ragSpeed;

    [SerializeField]
    private int _throwStrength = 500000;


    bool _waitForMouseRelease = false;

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

	public Vector3 temp;
    void Update()
    {
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
                            //Debug.Log(_wheelsMoved[i]);
                            if (Car.inst.wheels[i] == hit.transform && _wheelsMoved[i]==false)
                            {
                                _wheelsMoved[i] = true;
                                //Debug.Log("Found Mesh");
                                hit.transform.GetComponentInParent<FixWhealInPlace>().itsOk = true;
                                hit.transform.gameObject.SetActive(false);
                                Instantiate(wheal, hit.transform.position, wheal.transform.rotation).GetComponent<WhealStatus>()
                                    .theyAreNew = false;
                                AudioManager.AuidoManagerInstance.PlayOneShotRatchetAudio(1);
                            }
                        }
                    }
                    else if (hit.transform.tag == "WhealBox")
                    {
                        Instantiate(wheal, hit.point, wheal.transform.rotation).GetComponent<WhealStatus>()
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
						FireExtinguisherScript.inst.foaming = true;
						gridHeight = RedThingHeight;
						rb.isKinematic = true;
						rb.rotation = Quaternion.LookRotation(hit.point - rb.position) * Quaternion.Euler(temp);
					} else if (selected.tag == "Gas") {
						rb.isKinematic = true;
						gridHeight = gasHeight;
						ConnectGasHandle.inst.Detach();
					}
                    
                    rb.position = GetPointOnGrid(ray, gridHeight);

                    _ragSpeed = _oldRag - rb.position;
                    _oldRag = rb.position;
                }
			}
        }
        else if(selected != null && _waitForMouseRelease == false)
        {
			Rigidbody rb = selected.GetComponent<Rigidbody>();
			if (selected.tag == "RedThing")
            {
				FireExtinguisherScript.inst.foaming = false;
				rb.isKinematic = false;
			} if (selected.tag == "Gas") {
				if (ConnectGasHandle.inst.connected) {
					ConnectGasHandle.inst.Attach(selected.transform);
				} else {
					rb.isKinematic = false;
				}
			}

			//Debug.Log(_ragSpeed);
			if (rb.isKinematic == false) {
				rb.AddForce(-_ragSpeed * _throwStrength * Time.deltaTime, ForceMode.Force);
			}

			selected = null;

		}

		if (_waitForMouseRelease && isLeftMouseButtonPressed == false) {
			_waitForMouseRelease = false;
		}
    }
}
