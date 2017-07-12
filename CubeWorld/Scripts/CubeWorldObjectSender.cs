using UnityEngine;
using System.Collections;

public class CubeWorldObjectSender : MonoBehaviour {


	[SerializeField] int stencil;
	[SerializeField] string default_layer;
	[SerializeField] string current_layer;

	bool objectOverlapping = false;
	Transform drajObject;

	[SerializeField] float currentDot;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (objectOverlapping) {
			currentDot = Vector3.Dot(transform.up, drajObject.position - transform.position);

			if (currentDot < 0) {
				drajObject.gameObject.layer=LayerMask.NameToLayer (current_layer);
			} else 
				if (currentDot > 0)
					drajObject.gameObject.layer=LayerMask.NameToLayer (default_layer);
		}


	}


	void OnTriggerEnter(Collider other) {

		if (other.CompareTag("DragObject"))
		{
			objectOverlapping = true;
			drajObject = other.transform;
		}	
	}

	void OnTriggerExit(Collider other) {

		if (other.CompareTag("DragObject"))
		{
			objectOverlapping = false;
			drajObject = null;
		}	
	}


}
