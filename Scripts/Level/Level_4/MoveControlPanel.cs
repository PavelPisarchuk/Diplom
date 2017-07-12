using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControlPanel : MonoBehaviour {
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip abrotClip;


	[SerializeField] AudioSource[] _asClips;
	[SerializeField] AudioClip[] moveClip;

	[SerializeField] GameObject[] Stencil_1_OpenObject;
	[SerializeField] GameObject[] Stencil_1_CloseObject;

	[SerializeField] Animator[] anim;

	public bool Opened;
	public bool Usable;

	[SerializeField] float delayTime=3;
	float _delayTime=0;

	[SerializeField] bool ReAdress;
	[SerializeField] GameObject ReAdressObj;
	// Use this for initialization
	void Start () {
		_delayTime = delayTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ReAdressListener()
	{
		Usable = true;
	}

	public void ToDefaultState(){
		Opened = false;
		StopAllCoroutines ();
		SetStateDef ();
		_delayTime = Time.time;
	}

	void PlaySound()
	{
		for (int i = 0; i < _asClips.Length; i++) {
			float t = i;
			StartCoroutine (WaitCoroutine(_asClips[i],moveClip[i],t/4));
		}
	}

	void SetStateDef()
	{
		for (int i = 0; i < anim.Length; i++) {
			anim[i].SetBool ("On", Opened);
		}
		//anim[i].SetBool ("On", Opened);

		for(int i=0;i<Stencil_1_OpenObject.Length;i++)
			Stencil_1_OpenObject[i].SetActive (Opened);
		for(int i=0;i<Stencil_1_CloseObject.Length;i++)
			Stencil_1_CloseObject[i].SetActive (!Opened);


	}


	void SetState()
	{
		for (int i = 0; i < anim.Length; i++) {
			float t = i;
			StartCoroutine (WaitCoroutine2 (anim [i], Opened, t / 4));
		}
			//anim[i].SetBool ("On", Opened);

		for(int i=0;i<Stencil_1_OpenObject.Length;i++)
			Stencil_1_OpenObject[i].SetActive (Opened);
		for(int i=0;i<Stencil_1_CloseObject.Length;i++)
			Stencil_1_CloseObject[i].SetActive (!Opened);


	}


	IEnumerator WaitCoroutine(AudioSource _aus, AudioClip clp,float time)
	{
		yield return new WaitForSeconds(time);
		_aus.PlayOneShot (clp);
	}

	IEnumerator WaitCoroutine2(Animator anm,bool bl,float time)
	{
		yield return new WaitForSeconds(time);
		anm.SetBool ("On", bl);
	}



	void Event()
	{
		if (!Usable || !(_delayTime<Time.time)) {
			if (_as && abrotClip)
				_as.PlayOneShot (abrotClip);
		} else {


			Opened = !Opened;
			SetState ();
			PlaySound ();

			_delayTime = Time.time + delayTime;
		}


	}



	void MakeEvent()
	{
		if (ReAdress && ReAdressObj) {
			ReAdressObj.SendMessage ("ReAdressListener", SendMessageOptions.DontRequireReceiver);
			for(int i=0;i<_asClips.Length;i++)
				_asClips[i].PlayOneShot(moveClip[i]);
			return;
		} else
			Event ();
	}





}
