using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStateObject : MonoBehaviour {

	enum DefaultStateType{physicsObj, nonphysicsObj, hologramm, moveplatform  }
	[SerializeField] DefaultStateType stateType;
	[SerializeField] IKHead ikhead;
	[SerializeField] MoveControlPanel platform;

	Vector3 pos;
	Quaternion rot;
	bool isKinematic;
	Vector3 velocity;
	Vector3 angularvelocity;
	Rigidbody rig;
	bool usable;

	public void ToDefault(){
		if (stateType == DefaultStateType.physicsObj)
			PhysicsDefault ();

		if (stateType == DefaultStateType.hologramm)
			ikhead.DefaultStateMessage ();

		if (stateType == DefaultStateType.moveplatform)
			PlatformDefault ();
	}

	// Use this for initialization
	void Awake () {		
		if (stateType == DefaultStateType.physicsObj) {
			rig = GetComponent<Rigidbody> ();
			isKinematic = rig.isKinematic;
			velocity = rig.velocity;
			angularvelocity = rig.angularVelocity;

			pos = transform.position;
			rot = transform.rotation;
		}
		if (stateType == DefaultStateType.hologramm)
			ikhead = GetComponent<IKHead> ();
		
		if (stateType == DefaultStateType.moveplatform) {
			platform = GetComponent<MoveControlPanel> ();
			usable = platform.Usable;
		}
	}


	void PlatformDefault()
	{
		platform.Usable = usable;
		platform.ToDefaultState ();
	}

	void PhysicsDefault()
	{
		transform.position = pos;
		transform.rotation = rot;
		rig.isKinematic = isKinematic;
		rig.angularVelocity = velocity;
		rig.velocity = velocity;
	}

}
