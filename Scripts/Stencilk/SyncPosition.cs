using UnityEngine;
using System.Collections;

public class SyncPosition : MonoBehaviour {


	[SerializeField] Rigidbody SyncObj;
	Rigidbody myRig;
	// Use this for initialization
	void Start () {
		myRig = GetComponent<Rigidbody> ();
	}



	void SyncPos(Rigidbody body)
	{
		SyncObj.position = myRig.position;
		SyncObj.rotation = myRig.rotation;

		SyncObj.angularVelocity = myRig.angularVelocity;
		SyncObj.velocity = myRig.velocity;
	}

	void OnCollisionEnter(Collision other)
	{
		//SyncPos (myRig);
	}

}
