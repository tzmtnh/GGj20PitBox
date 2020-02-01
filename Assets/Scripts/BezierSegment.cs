using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BezierSegment : MonoBehaviour {
	Vector3[] _points;
	[SerializeField, HideInInspector] Transform[] _controlPoints;

	public Vector3 getPos(float t) {
		if (t < 0f) t = 0f; else if (t > 1f) t = 1f;
		float t2 = t * t;
		float t3 = t2 * t;

		float r = 1f - t;
		float r2 = r * r;
		float r3 = r2 * r;

		float rt2 = 3f * t2 * r;
		float r2t = 3f * t * r2;

		return  (r * r2) * _points[0] +
				(3f * r2 * t) * _points[1] +
				(3f * r * t2) * _points[2] +
				(t * t2) * _points[3];
	}

	void Init() {
		if (_controlPoints != null && _controlPoints[0] != null) return;

		_points = new Vector3[4];
		_controlPoints = new Transform[4];
		for (int i = 0; i < 4; i++) {
			GameObject go = new GameObject();
			go.name = "Point " + i;
			go.transform.SetParent(transform);
			_controlPoints[i] = go.transform;
		}

		_controlPoints[1].SetParent(_controlPoints[0]);
		_controlPoints[2].SetParent(_controlPoints[3]);
	}

	void Awake() {
		Init();
		_points = new Vector3[4];
	}

	void Update() {
		Init();

		for (int i = 0; i < _points.Length; i++) {
			_points[i] = _controlPoints[i].position;
		}
	}

	public void OnDrawGizmos() {
		const float SAMPLES = 20;

		Gizmos.color = Color.yellow;
		Vector3 lastPos = getPos(0);
		for (int i = 1; i < SAMPLES; i++) {
			float t = i / (SAMPLES - 1f);
			Vector3 pos = getPos(t);
			Gizmos.DrawLine(lastPos, pos);
			lastPos = pos;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(_points[0], _points[1]);
		Gizmos.DrawLine(_points[3], _points[2]);
	}
}
