using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExt : MonoBehaviour
{
	public static FireExt inst;

	public bool foaming = false;
	public ParticleSystem foam;

	void Awake() {
		inst = this;
	}

	void Update() {
		Car.ToggleEmission(foam, foaming);
	}
}
