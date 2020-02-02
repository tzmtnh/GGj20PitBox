using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGasHandle : MonoBehaviour
{
	public static ConnectGasHandle inst;

	Transform _anchor;

	bool _connected = false;
	public bool connected { get { return _connected; } }

	bool _canConnect = true;
    [SerializeField]
    private float _fuelPerSecond = 30;

	void Awake() {
		inst = this;
		_anchor = transform.Find("Anchor");
	}

	void OnTriggerEnter(Collider other)
    {
		if (_canConnect == false) return;
		if (_connected) return;
		if (other.tag != "Gas") return;

        other.transform.position = _anchor.position;
        other.transform.rotation = _anchor.rotation;
        other.GetComponent<Rigidbody>().isKinematic = true;
		
		NewMouseDrag.inst.WaitForMouseRelease();

        _connected = true;
		_canConnect = false;

        AudioManager.AuidoManagerInstance.PlayingFuelingAudio(true,1);

	}

	void OnTriggerExit(Collider other) {
		if (_connected == false) return;
		if (other.tag != "Gas") return;

		other.GetComponent<Rigidbody>().isKinematic = false;
		_connected = false;
        AudioManager.AuidoManagerInstance.PlayingFuelingAudio(false, 1);
    }

    void Update() {
        if (_connected)
        {
			Simulation.SimulationInst.FuelRefil(_fuelPerSecond*Time.deltaTime);
        }

        if (_canConnect == false && _connected == false && Input.GetMouseButtonUp(0)) {
			_canConnect = true;
		}
	}
}
