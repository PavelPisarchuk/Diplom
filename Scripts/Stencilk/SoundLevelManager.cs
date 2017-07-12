using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Level))]
public class SoundLevelManager : MonoBehaviour {
	[SerializeField] Level lvl;
	[SerializeField] CameraEffects camEffect;

	[SerializeField] AudioSource[] as_0;
	[SerializeField] AudioSource[] as_1;

	float speed;

	bool needSwap;
	bool toZero;
	// Use this for initialization
	void Awake () {
		lvl = GetComponent<Level> ();
		camEffect = (CameraEffects)FindObjectOfType (typeof(CameraEffects));
		ToDefault ();
	}

	void Update()
	{
		if (needSwap) {
			if (toZero)
				ToZero ();
			else
				ToOne ();
		}
	}

	public void Swap(int time,bool onStart)
	{
		if (as_0.Length == 0 || as_1.Length == 0)
			return;
		if (time == 1) {
			toZero = false;
			needSwap = true;
			ToDefault ();
		} else if (time == 0) {
			toZero = true;
			needSwap = true;
			ToOneFast ();
		}
	}

	public void SwapFast(int time,bool onStart)
	{
		if (as_0.Length == 0 || as_1.Length == 0)
			return;
		if (time == 1) {
			toZero = false;
			needSwap = false;
			ToOneFast ();
		} else if (time == 0) {
			toZero = true;
			needSwap = false;
			ToDefault ();
		}
	}

	void ToZero( )
	{
		if (as_0 [0].volume == 1) {
			needSwap = false;
			return;
		}

		if (as_1 [0].volume != 0) {//0 - to zero volume

			for (int i = 0; i < as_1.Length; i++) {
				as_1 [i].volume = Mathf.Clamp(Mathf.Lerp(as_1 [i].volume,-0.1f,Time.deltaTime*3), 0 , 1)  ;
			}

		} else {

			for (int i = 0; i < as_0.Length; i++) {
				as_0 [i].volume = Mathf.Clamp(Mathf.Lerp(as_0 [i].volume,1.1f,Time.deltaTime*3), 0 , 1)  ;
			}

		}

	}

	void ToOne( )
	{
		if (as_1 [0].volume == 1) {
			needSwap = false;
			return;
		}

		if (as_0 [0].volume != 0) {//0 - to zero volume
			
			for (int i = 0; i < as_0.Length; i++) {
				as_0 [i].volume = Mathf.Clamp(Mathf.Lerp(as_0 [i].volume,-0.1f,Time.deltaTime*3), 0 , 1)  ;
			}

		} else {

			for (int i = 0; i < as_1.Length; i++) {
				as_1 [i].volume = Mathf.Clamp(Mathf.Lerp(as_1 [i].volume,1.1f,Time.deltaTime*3), 0 , 1)  ;
			}

		}


	}


	public void ToDefault()
	{

		for (int i = 0; i < as_1.Length; i++) {
			as_1 [i].volume = 0;
		}
		for (int i = 0; i < as_0.Length; i++) {
			as_0 [i].volume = 1;
		}
	}


	void ToOneFast()
	{

		for (int i = 0; i < as_1.Length; i++) {
			as_1 [i].volume = 1;
		}
		for (int i = 0; i < as_0.Length; i++) {
			as_0 [i].volume = 0;
		}
	}

}
