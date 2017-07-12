using UnityEngine;
using System.Collections;

public class HandSmouth : MonoBehaviour {
	Vector3 def;
	[SerializeField] float smooth;
	[SerializeField] float amount;
	[SerializeField] float MaxAmount;

	// Use this for initialization
	void Awake () {
		def = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		float forceX = -Input.GetAxis("Mouse X") * amount;
		float forceY = -Input.GetAxis("Mouse Y") * amount;

		if (forceX > MaxAmount)
			forceX = MaxAmount;
		
		if (forceX < -MaxAmount)
			forceX = -MaxAmount;
		
		if (forceY > MaxAmount)
			forceY = MaxAmount;
		
		if (forceY < -MaxAmount)
			forceY = -MaxAmount;

		Vector3 end = new Vector3(def.x+forceX, def.y+forceY, def.z);
		transform.localPosition = Vector3.Lerp(transform.localPosition, end, Time.deltaTime * smooth);

	}
}
