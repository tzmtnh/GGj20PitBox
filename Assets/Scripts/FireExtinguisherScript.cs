using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FireExtinguisherScript : MonoBehaviour
{
	public static FireExtinguisherScript inst;

	public bool foaming = false;
	public ParticleSystem foam;

	private bool _isFixing = false;
    [SerializeField]
    private float _extinguishRate = 20;

	Rigidbody _rigidbody;
	Vector3 _initPos;
	Quaternion _initRot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CarEngineArea" && Simulation.SimulationInst.EngineCanCoolOff())
        {
            _isFixing = true;
           AudioManager.AuidoManagerInstance.PlayingFireExtinguisherAudio(true,1);
        }

        if (other.tag == "KillTag")
        {
			_rigidbody.position = _initPos;
			_rigidbody.rotation = _initRot;
			_rigidbody.velocity = new Vector3();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CarEngineArea")
        {
            _isFixing = false;
            AudioManager.AuidoManagerInstance.PlayingFireExtinguisherAudio(false, 1);
        }
    }

	void Awake() {
		inst = this;
		_rigidbody = GetComponent<Rigidbody>();
		_initPos = transform.position;
		_initRot = transform.rotation;
	}

	private void Update()
    {
		Car.ToggleEmission(foam, foaming);

		if (_isFixing)
        {
            Simulation.SimulationInst.EngineRepair(_extinguishRate*Time.deltaTime);
        }
    }
}
