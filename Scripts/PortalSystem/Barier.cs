using UnityEngine;
using System.Collections;

public class Barier : MonoBehaviour {
	[SerializeField] float force;
	DragObjects drag;
	void Start()
	{
		drag = GameObject.FindGameObjectWithTag ("Player").GetComponent<DragObjects>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("DragObject")) {
			Rigidbody rig = other.GetComponent<Rigidbody> ();
			drag.ClearObjWithoutImpuls ();
			rig.AddForce (transform.right * force);
		}

	}

}
