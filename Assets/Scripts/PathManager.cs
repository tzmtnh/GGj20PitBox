using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	public static PathManager inst;

	public BezierSegment inSpline;
	public BezierSegment outSpline;
	public BezierSegment roadSpline;

	void Awake() {
		inst = this;
	}
}
