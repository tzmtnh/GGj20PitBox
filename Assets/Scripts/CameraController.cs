using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float sensitivity = 2;
	public float drag = 0.5f;

	Camera _camera;
	Transform _pitch;
	Vector2 _lastPos;

	float _orbitAngle;
	float _orbitVelocity;

	float _pitchAngle;
	float _pitchVelocity;

	void Awake() {
		_pitch = transform.Find("Camera Pitch");
		_camera = _pitch.GetComponentInChildren<Camera>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(1)) {
			_lastPos = Input.mousePosition;
		}

		float dt = Time.deltaTime;
		if (Input.GetMouseButton(1)) {
			Vector2 pos = Input.mousePosition;
			Vector2 delta = pos - _lastPos;

			_orbitVelocity = delta.x * sensitivity;
			_pitchVelocity = -delta.y * sensitivity;
		} else {
			_orbitVelocity *= (1f - drag);
			_pitchVelocity *= (1f - drag);
		}

		_orbitAngle += _orbitVelocity * dt;
		_pitchAngle += _pitchVelocity * dt;
		_pitchAngle = Mathf.Clamp(_pitchAngle, 5, 90);

		transform.localRotation = Quaternion.Euler(0, _orbitAngle, 0);
		_pitch.localRotation = Quaternion.Euler(_pitchAngle, 0, 0);
	}
}
