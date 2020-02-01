using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	public float animationDuration = 3;
	public BezierSegment inSpline;
	public BezierSegment outSpline;
	public ParticleSystem dust;

	Vector3 _velocity;
	public Vector3 velocity { get { return _velocity; } }
	public float speed { get { return _velocity.magnitude; } }

	Transform _transform;
	Transform _steer;
	Transform _frontWheels;
	Transform _rearWheels;

	Vector3 _lastPos;
	bool _initialized = false;

	Vector3 _dir;
	Vector3 _dirVelocity;

	float _steerAngle;
	float _steerVelocity;

	const float WHEEL_RADIUS = 0.35f;

	public void EnterPit() {
		FollowSpline(animationDuration, inSpline);
	}

	public void ExitPit() {
		FollowSpline(animationDuration, outSpline);
	}

	void FollowSpline(float duration, BezierSegment segment) {
		if (_followSplineCo != null) {
			StopCoroutine(_followSplineCo);
		}
		_followSplineCo = StartCoroutine(FollowSplineCo(duration, segment));
	}

	Coroutine _followSplineCo = null;
	IEnumerator FollowSplineCo(float duration, BezierSegment segment) {
		float timer = 0;
		while (timer < duration) {
			float t = Mathf.SmoothStep(0, 1, timer / duration);
			_transform.position = segment.getPos(t);
			yield return null;
			timer += Time.deltaTime;
		}

		_transform.position = segment.getPos(1);
		_followSplineCo = null;
	}

    void Awake()
    {
        _transform = transform;
        _steer = _transform.Find("Steer");
        _frontWheels = _steer.Find("FrontWheels");
        _rearWheels = _transform.Find("RearWheels");
    }
    void Start()
    { 
        EnterPit();
	}
	
	void Update() {
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

		var emission = dust.emission;
		emission.rateOverTimeMultiplier = Mathf.Min(_velocity.magnitude, 1) * 50;
	}
}
