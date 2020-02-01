using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGasHandle : MonoBehaviour
{
	bool _connected = false;
	bool _canConnect = true;

	void OnTriggerEnter(Collider other)
    {
		if (_canConnect == false) return;
		if (_connected) return;
		if (other.tag != "Gas") return;

        other.transform.position = transform.position;
        other.transform.rotation = transform.rotation;
        other.GetComponent<Rigidbody>().isKinematic = true;
		
		NewMouseDrag.inst.WaitForMouseRelease();

        _connected = true;
		_canConnect = false;
    }

	void OnTriggerExit(Collider other) {
		if (_connected == false) return;
		if (other.tag != "Gas") return;

		other.GetComponent<Rigidbody>().isKinematic = false;
		_connected = false;
	}

	void Update() {
		if (_canConnect == false && _connected == false && Input.GetMouseButtonUp(0)) {
			_canConnect = true;
		}
	}
}
