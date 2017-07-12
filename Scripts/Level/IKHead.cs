using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHead : MonoBehaviour {
	
	[SerializeField] Animator anim;
	[SerializeField] Transform Player;
	[SerializeField] Transform SelfPos;
	[SerializeField] Vector3 finalPos;
	[SerializeField] float minDistance=25;
	[SerializeField] float speed=1;

	[Space(20)]
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip talkSound;
	[SerializeField] float talkingTime;
	[SerializeField] bool talkingNow;

	[SerializeField] GameObject EventObj;
	[SerializeField] bool EventAfterTalk;
	[SerializeField] bool EventIgnoreTalk;
	GameObject plr;
	public bool NeedRotateHead {
		get;
		set;
	}

	void Awake () {
		//SelfPos.position = transform.position;
		NeedRotateHead = false;
		anim = GetComponent<Animator> ();
		_as=GetComponent<AudioSource> ();
		Player = GameObject.FindGameObjectWithTag ("MainCamera").transform;
		plr = GameObject.FindGameObjectWithTag ("Player");
	}
	void Start()
	{

	}
	
	void Update () {
		if (Vector3.Distance (transform.position, Player.position) < minDistance && NeedRotateHead && plr.layer==gameObject.layer) {
			finalPos = Vector3.Lerp (finalPos, Player.position, Time.deltaTime * speed);
		}
		else
			finalPos = Vector3.Lerp (finalPos, SelfPos.position, Time.deltaTime * speed);
		
	}

	void OnAnimatorIK(int layerIndex) {		
			anim.SetLookAtWeight (1);
			anim.SetLookAtPosition (finalPos);
	}

	void MakeEvent()
	{
		if(EventIgnoreTalk && EventObj!=null)
			EventObj.SendMessage ("MakeEvent",SendMessageOptions.DontRequireReceiver);
		
		if (talkingNow)
			return;
		talkingNow = true;

		if(_as!=null)
			_as.PlayOneShot (talkSound);
		if(!EventAfterTalk && EventObj!=null && !EventIgnoreTalk)
			EventObj.SendMessage ("MakeEvent",SendMessageOptions.DontRequireReceiver);
		StartCoroutine (TalkCoroutine());
	}


	IEnumerator TalkCoroutine()
	{
		NeedRotateHead = true;
		yield return new WaitForSeconds(talkingTime);
		if(EventObj!=null && EventAfterTalk && !EventIgnoreTalk)
			EventObj.SendMessage ("MakeEvent",SendMessageOptions.DontRequireReceiver);
		NeedRotateHead = false;
		talkingNow = false;
	}

	public void DefaultStateMessage()
	{
		StopCoroutine (TalkCoroutine());
		NeedRotateHead = false;
		talkingNow = false;
		_as.Stop ();
	}

}
