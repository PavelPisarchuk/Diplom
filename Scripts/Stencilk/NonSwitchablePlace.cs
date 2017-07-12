using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSwitchablePlace : MonoBehaviour {
	StencilSwithcer swithcer;

	void Start()
	{
		swithcer =(StencilSwithcer) FindObjectOfType (typeof(StencilSwithcer));
	}

	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag ("Player")) {
			swithcer.UsabaleInPlace = false;
		}

	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Player")) {
			swithcer.UsabaleInPlace = true;
		}

	}

}
