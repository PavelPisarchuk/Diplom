using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	[SerializeField] StencilSwithcer switcher;
	[SerializeField] HandController handController;
	[SerializeField] EventRay eventRay;
	// Use this for initialization
	void Awake () {
		switcher = (StencilSwithcer)FindObjectOfType(typeof(StencilSwithcer));
		handController = (HandController)FindObjectOfType(typeof(HandController));
		eventRay = (EventRay)FindObjectOfType(typeof(EventRay));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			switcher.SwapTime();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			switcher.EnabledScreen ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			handController.FlashLight ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {

		}
	}
}
