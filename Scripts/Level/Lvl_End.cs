using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl_End : MonoBehaviour {
	enum Level_End_Type { Player_Trigger, Object_Trigger, Press_Trigger };
	[SerializeField] Level_End_Type endType;
	[SerializeField] Level lvl;
	[SerializeField] LevelManager lvlManager;
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip levelEndCLip;
	[SerializeField] Transform cubePos;
	GameObject obj;
	Rigidbody rig;

	// Use this for initialization
	void Awake () {
		lvlManager = (LevelManager)FindObjectOfType (typeof(LevelManager));
		_as = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (obj != null) {
			PhysicsCubeToPos ();
		}

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			if (endType == Level_End_Type.Player_Trigger)
				LevelEnd ();			
		}
		if (other.CompareTag ("DragObject")) {
			if (endType == Level_End_Type.Object_Trigger && other.GetComponent<LvlEndObject> () != null) {
				//PhysicsCubeToPos (other.gameObject);
				obj=other.gameObject;
				rig = obj.GetComponent<Rigidbody> ();
				rig.isKinematic = true;
				LevelEnd ();
			}

		}

	}

	void PhysicsCubeToPos()
	{		
		obj.transform.position = Vector3.Lerp (obj.transform.position, cubePos.position, Time.deltaTime * 2);
		obj.transform.rotation = Quaternion.Lerp (obj.transform.rotation, cubePos.rotation, Time.fixedDeltaTime * 3);
	}

	void LevelEnd()
	{
		_as.PlayOneShot (levelEndCLip);
		lvlManager.EndCurrentLevel (lvl.Id);
	}

	public void Reset()
	{
		obj = null;
		rig = null;
	}

	void PressEvent()
	{
		if (endType == Level_End_Type.Press_Trigger)
			LevelEnd ();
	}


}
