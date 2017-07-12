using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {
	[SerializeField] DragObjects drag;
	[SerializeField] StencilSwithcer switcher;
	[SerializeField] AnimController player;
	[SerializeField] GameObject flashLight;
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip flashLightCLip;
	// Use this for initialization
	void Awake () {
		drag = GetComponent<DragObjects> ();
		player = GetComponent<AnimController> ();
		OffLeftHand ();
		OffRightHand ();
	}
	
	// Update is called once per frame
	void Update () {
		

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			OffLeftHand ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			OffRightHand ();
		}



		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			OnLeftHand ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			OnRightHand ();
		}
	}

	public void FlashLight()
	{
		if (switcher.Usebale == false)
			flashLight.SetActive (false);
		else {
			flashLight.SetActive (!flashLight.activeSelf);
			if(flashLightCLip)
				_as.PlayOneShot (flashLightCLip);
		}
	}

	public void OffLeftHand()
	{
		player.OffLeftHand ();
		switcher.Usebale = false;
		flashLight.SetActive (false);
	}
	public void OnLeftHand()
	{
		player.OnLeftHand ();
		switcher.Usebale = true;
		//flashLight.SetActive (true);
	}


	public void OffRightHand()
	{
		player.OffRightHand ();
		drag.ClearObjWithoutImpuls ();
		drag.enabled = false;
	}
	public void OnRightHand()
	{
		player.OnRightHand ();
		drag.enabled = true;
	}



	void Messager(GameObject target,string method)
	{
		target.SendMessage (method, SendMessageOptions.DontRequireReceiver);
	}

}
