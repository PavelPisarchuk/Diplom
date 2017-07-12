using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
public class AnimController : MonoBehaviour {
	[SerializeField] Animator leftHand;
	[SerializeField] Animator rightHand;
	[SerializeField] FirstPersonController controller;
	bool canMove=true;
	public bool CanMove{
		set{canMove = value; }
	}
	// Use this for initialization
	void Awake () {
		controller = GetComponent<FirstPersonController> ();
	}

	// Update is called once per frame
	void Update () {

		//rightHand.speed = controller.Grounded ? 1 : 0;
		//leftHand.speed = rightHand.speed;//controller.Grounded ? 1 : 0;

	}

	void FixedUpdate()
	{
		return;
		if (!canMove) {
			leftHand.SetFloat ("Move", 0);
			rightHand.SetFloat ("Move", 0);
			return;
		}

		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");


		if ((horizontal == 0 && vertical == 0)) {
			float t = Mathf.Lerp (leftHand.GetFloat ("Move"), 0, Time.deltaTime * 5);
			leftHand.SetFloat ("Move", t);
			rightHand.SetFloat ("Move", t);

		} else {
			float t = Mathf.Lerp (leftHand.GetFloat ("Move"), 1, Time.deltaTime * 5);			
			leftHand.SetFloat ("Move", t);
			rightHand.SetFloat ("Move", t);

		}

	}



	public void OffLeftHand()
	{
		leftHand.ResetTrigger("On");
		leftHand.SetTrigger ("Off");
	}
	public void OnLeftHand()
	{
		leftHand.ResetTrigger("Off");
		leftHand.SetTrigger ("On");
		//leftHand.SetTime(rightHand.GetTime());
	}


	public void OffRightHand()
	{
		rightHand.ResetTrigger("On");
		rightHand.SetTrigger ("Off");
	}
	public void OnRightHand()
	{
		rightHand.ResetTrigger("Off");
		rightHand.SetTrigger ("On");
		//rightHand.SetTime(leftHand.GetTime());
	}



	IEnumerator WaitCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
	}

}
