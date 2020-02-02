using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGasHandle : MonoBehaviour
{
	public static ConnectGasHandle inst;

	Transform _anchor;
	Transform _attachment;
	Transform _oldParent;

	public bool connected { get; private set; }

	public bool broken { get; set; }

	bool _canConnect = true;
    [SerializeField]
    private float _fuelPerSecond = 30;

	public void Attach(Transform t) {
		if (_attachment != null) return;
		_attachment = t;
		_oldParent = t.parent;
		t.SetParent(_anchor);
	}

	public void Detach() {
		if (_attachment == null) return;
		_attachment.SetParent(_oldParent);
		_attachment = null;
		_oldParent = null;
	}

	void Awake() {
		Car car = GetComponentInParent<Car>();
		if (car.isPlayer == false) {
			Destroy(this);
			return;
		}

		inst = this;
		_anchor = transform.Find("Anchor");
	}

	void OnTriggerEnter(Collider other)
    {
		if (_canConnect == false) return;
		if (connected) return;
		if (other.tag != "Gas") return;

        other.transform.position = _anchor.position;
        other.transform.rotation = _anchor.rotation;
        other.GetComponent<Rigidbody>().isKinematic = true;
		
		NewMouseDrag.inst.WaitForMouseRelease();

        connected = true;
		_canConnect = false;

        AudioManager.AuidoManagerInstance.PlayingFuelingAudio(true,1);

	}

	void OnTriggerExit(Collider other) {
		if (connected == false) return;
		if (other.tag != "Gas") return;

		other.GetComponent<Rigidbody>().isKinematic = false;

		connected = false;
        AudioManager.AuidoManagerInstance.PlayingFuelingAudio(false, 1);
    }

    void Update() {
        if (connected && broken == false) {
			Simulation.SimulationInst.FuelRefil(_fuelPerSecond*Time.deltaTime);
        }

        if (_canConnect == false && connected == false && Input.GetMouseButtonUp(0)) {
			_canConnect = true;
		}
	}
}
