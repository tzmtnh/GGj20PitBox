using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	public static Car inst;

	[Header("Params")]
	public float animationDuration = 3;
	[Range(0, 1)] public float fireStrength = 0;

	[Header("Particles")]
	public ParticleSystem dust;
	public ParticleSystem fire;
	public ParticleSystem[] sparks;

	[Header("Wheels")]
	public Transform[] wheels;

	[Header("Path")]
	public BezierSegment inSpline;
	public BezierSegment outSpline;

	Vector3 _velocity;
	public Vector3 velocity { get { return _velocity; } }
	public float speed { get { return _velocity.magnitude; } }

	Light _fireLight;

	Transform _transform;
	Transform _steer;
	Transform _frontWheels;
	Transform _rearWheels;
	Transform _chassis;

	Vector3 _lastPos;
	bool _initialized = false;

	Vector3 _dir;
	Vector3 _dirVelocity;

	float _steerAngle;
	float _steerVelocity;

	const float WHEEL_RADIUS = 0.35f;

	public void EnterPit(float startParam = 0) {
		FollowSpline(animationDuration, startParam, inSpline);
	}

	public void ExitPit() {
		FollowSpline(animationDuration, 0, outSpline);
	}

	void FollowSpline(float duration, float startParam, BezierSegment segment) {
		if (_followSplineCo != null) {
			StopCoroutine(_followSplineCo);
		}
		_followSplineCo = StartCoroutine(FollowSplineCo(duration, startParam, segment));
	}

	Coroutine _followSplineCo = null;
	IEnumerator FollowSplineCo(float duration, float startParam, BezierSegment segment)
    {
        float timer = duration * startParam;

		dust.Stop();
		_transform.position = segment.getPos(timer / duration);
		dust.Play();

		AudioManager.AuidoManagerInstance.PlayingCarEngineAudio(true, 1);
		while (timer < duration) {
			float t = Mathf.SmoothStep(0, 1, timer / duration);
			_transform.position = segment.getPos(t);
			yield return null;
			timer += Time.deltaTime;
		}
        AudioManager.AuidoManagerInstance.PlayingCarEngineAudio(false, 1);
        _transform.position = segment.getPos(1);
		_followSplineCo = null;
	}

	void UpdateWheels() {
		float dt = Time.deltaTime;
		Vector3 pos = _transform.position;
		Vector3 delta = pos - _lastPos;
		float dist = delta.magnitude;
		bool reversed = Vector3.Dot(_transform.up, delta) < 0;

		if (dist > 0.001f) {
			Vector3 lastForward = _transform.up;

			Vector3 dir = delta.normalized;
			if (_initialized == false) {
				_dir = dir;
			} else {
				_dir = Vector3.SmoothDamp(_dir, dir, ref _dirVelocity, 0.3f);
			}
			_transform.rotation = Quaternion.LookRotation(_dir) * Quaternion.Euler(-90, 0, 180);

			Vector3 forward = _transform.up;
			float steerAngle = 5 * Vector3.Angle(lastForward, forward) * Mathf.Sign(Vector3.Cross(lastForward, forward).y);
			if (_initialized == false) {
				_steerAngle = steerAngle;
			} else {
				_steerAngle = Mathf.SmoothDamp(_steerAngle, steerAngle, ref _steerVelocity, 0.3f);
			}
			_steer.localRotation = Quaternion.Euler(0, 0, _steerAngle);
			_initialized = true;
		}

		float wheelsRotation = Mathf.Rad2Deg * delta.magnitude / WHEEL_RADIUS * (reversed ? 1 : -1);
		_frontWheels.Rotate(wheelsRotation, 0, 0);
		_rearWheels.Rotate(wheelsRotation, 0, 0);

		_velocity = delta / dt;
		_lastPos = pos;
	}

	void UpdateFire() {
		var emission = fire.emission;
		emission.rateOverTimeMultiplier = fireStrength * 50;

		float t = Mathf.PerlinNoise(Time.time * 10, 1.234f);
		_fireLight.enabled = fireStrength > 0.01f;
		_fireLight.intensity = Mathf.Lerp(0.5f, 1f, t) * fireStrength * 5f;
	}

	public static void ToggleEmission(ParticleSystem system, bool state) {
		var emission = system.emission;
		emission.enabled = state;
	}

	void UpdateSparks() {
		bool isMoving = speed > 0.5f;

		bool sparksFrontRight =	isMoving && !wheels[0].gameObject.activeInHierarchy;
		bool sparksFrontLeft =	isMoving && !wheels[1].gameObject.activeInHierarchy;
		bool sparksRearRight =	isMoving && !wheels[2].gameObject.activeInHierarchy;
		bool sparksRearLeft =	isMoving && !wheels[3].gameObject.activeInHierarchy;

		ToggleEmission(sparks[0], sparksFrontRight);
		ToggleEmission(sparks[1], sparksFrontLeft);
		ToggleEmission(sparks[2], sparksRearRight);
		ToggleEmission(sparks[3], sparksRearLeft);
	}

	void UpdateChassis() {
		float t = Time.time;
		float z = 0.005f * Mathf.Sin(37 * t);
		_chassis.localPosition = new Vector3(0, 0, z);

		float angle = 0.5f * Mathf.Sin(11 * t); ;
		_chassis.localRotation = Quaternion.Euler(0, angle, 0);
	}

    void Awake()
    {
		inst = this;
        _transform = transform;
        _steer = _transform.Find("Steer");
        _frontWheels = _steer.Find("FrontWheels");
        _rearWheels = _transform.Find("RearWheels");
		_fireLight = fire.GetComponentInChildren<Light>();
		_chassis = _transform.Find("Chassis");
	}

	void Update() {
        if (!Simulation.SimulationInst.IsGameRunning()) return;

		UpdateWheels();
		UpdateChassis();
		UpdateFire();
		UpdateSparks();
	}
}
