using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPump : MonoBehaviour
{
	public Transform[] bones;

	Transform _handle;
	Vector3 _tubeOrigin;

	void SetupTube() {
		Rigidbody parentRB = null;
		Transform root = bones[0].parent;
		if (root != null) {
			parentRB = root.gameObject.GetComponent<Rigidbody>();
			if (parentRB == null) {
				parentRB = root.gameObject.AddComponent<Rigidbody>();
				parentRB.isKinematic = true;
			}
		}

		foreach (Transform bone in bones) {
			bone.SetParent(root);
		}

		for (int i = 0; i < bones.Length; i++) {
			Transform t = bones[i];
			Transform p = t.parent;

			CapsuleCollider capsule = t.gameObject.AddComponent<CapsuleCollider>();
			capsule.center = new Vector3(-0.025f, 0, 0);
			capsule.radius = 0.01f;
			capsule.height = 0.05f;
			capsule.direction = 0;
			
			Rigidbody rb = t.gameObject.AddComponent<Rigidbody>();
			rb.useGravity = true;
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			rb.detectCollisions = false;
			
			ConfigurableJoint joint = rb.gameObject.AddComponent<ConfigurableJoint>();
			joint.connectedBody = parentRB;
			joint.axis = Vector3.forward;
			joint.secondaryAxis = Vector3.up;
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Free;
			joint.angularYMotion = ConfigurableJointMotion.Free;
			joint.angularZMotion = ConfigurableJointMotion.Free;
			//joint.enableCollision = true;
			joint.rotationDriveMode = RotationDriveMode.Slerp;

			var slerpDrive = joint.slerpDrive;
			slerpDrive.maximumForce = Mathf.Infinity;
			slerpDrive.positionSpring = 100;
			slerpDrive.positionDamper = 10;
			joint.slerpDrive = slerpDrive;

			parentRB = rb;
		}

		{
			ConfigurableJoint joint = parentRB.gameObject.AddComponent<ConfigurableJoint>();
			joint.connectedBody = _handle.GetComponent<Rigidbody>();
			joint.axis = Vector3.forward;
			joint.secondaryAxis = Vector3.up;
			joint.xMotion = ConfigurableJointMotion.Locked;
			joint.yMotion = ConfigurableJointMotion.Locked;
			joint.zMotion = ConfigurableJointMotion.Locked;
			joint.angularXMotion = ConfigurableJointMotion.Free;
			joint.angularYMotion = ConfigurableJointMotion.Free;
			joint.angularZMotion = ConfigurableJointMotion.Free;
			joint.enableCollision = true;
			joint.rotationDriveMode = RotationDriveMode.Slerp;

			var slerpDrive = joint.slerpDrive;
			slerpDrive.maximumForce = Mathf.Infinity;
			slerpDrive.positionSpring = 100;
			slerpDrive.positionDamper = 10;
			joint.slerpDrive = slerpDrive;
		}
	}

	void Awake() {
		_handle = bones[0].parent.Find("Handle");
		SetupTube();
	}
}
