using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float sensitivity = 2;
	public float drag = 0.5f;

	public Vector3 zoomRange = new Vector3(2, 5, 3);
	public float zoomSensitivity = 1;

	Camera _camera;
	Transform _pitch;
	Transform _zoom;
	Vector2 _lastPos;

	float _zoomParam;
	float _zoomTarget;
	float _zoomVelocity;

	float _orbitAngle;
	float _orbitVelocity;

	float _pitchAngle;
	float _pitchVelocity;

	void Awake() {
		_pitch = transform.Find("Camera Pitch");
		_camera = _pitch.GetComponentInChildren<Camera>();
		_zoom = _camera.transform;

		_orbitAngle = -45;
		_pitchAngle = 30;
		_zoomParam = Mathf.InverseLerp(zoomRange.x, zoomRange.y, zoomRange.z);
		_zoomTarget = _zoomParam;
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

		Vector3 scrollDelta = Input.mouseScrollDelta;
		_zoomTarget -= dt * zoomSensitivity * scrollDelta.y;
		_zoomTarget = Mathf.Clamp01(_zoomTarget);
		_zoomParam = Mathf.SmoothDamp(_zoomParam, _zoomTarget, ref _zoomVelocity, 0.3f);
		float zoom = -Mathf.Lerp(zoomRange.x, zoomRange.y, Mathf.SmoothStep(0, 1, _zoomParam));
		_zoom.localPosition = new Vector3(0, 0, zoom);
	}
}
