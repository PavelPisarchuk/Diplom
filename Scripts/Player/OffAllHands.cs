using UnityEngine;
using System.Collections;

public class OffAllHands : MonoBehaviour {

	[SerializeField] HandController hands;
	[SerializeField] StencilSwithcer swithcer;
	[SerializeField] bool on_off;
	[SerializeField] bool onlyRight;

	void Awake()
	{
		hands = GameObject.FindGameObjectWithTag ("Player").GetComponent<HandController> ();
		swithcer = (StencilSwithcer) FindObjectOfType (typeof(StencilSwithcer));
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Player")) {
			if (on_off) {
				hands.OnRightHand ();
				if (!onlyRight) {
					hands.OnLeftHand ();
				}
			} else {
				hands.OffLeftHand ();
				hands.OffRightHand ();
				//swithcer.ToDefaultWithEffect ();
			}


		}


	}

}
