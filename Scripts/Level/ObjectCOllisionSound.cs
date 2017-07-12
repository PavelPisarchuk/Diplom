using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCOllisionSound : MonoBehaviour {
	[SerializeField] AudioClip[] clips;
	[SerializeField] AudioSource _as;
	[SerializeField] float deadLineValue=1.75f;
	// Use this for initialization
	void Start () {
		_as = GetComponent<AudioSource> ();
	}


	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag ("Player"))
			return;
		else if (other.relativeVelocity.magnitude > deadLineValue) {
			_as.PlayOneShot (clips [Random.Range (0, clips.Length)]);
		}
	}

}
