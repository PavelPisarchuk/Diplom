using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventRay : MonoBehaviour {
	RaycastHit hit;
	bool flag=false;
	[SerializeField] GameObject eventTextObj;
	[SerializeField] float distance=3;
	GameObject Player;

	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip[] BackgroundClips;
	int currentSound=0;
	// Use this for initialization
	void Awake () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		//_as=GetComponent<AudioSource> ();
		currentSound = UnityEngine.Random.Range (0, BackgroundClips.Length-1);
	}

	void PlayeNextSound()
	{
		_as.clip = BackgroundClips [currentSound];
		_as.Play ();
		if (currentSound + 1 >= BackgroundClips.Length)
			currentSound = 0;
		else
			currentSound++;
	}

	// Update is called once per frame
	void Update () {
		if (!_as.isPlaying) {
			PlayeNextSound ();
		}

		Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
		if (Physics.Raycast (ray, out hit, distance,1<<Convert.ToInt32 (Player.layer))) {
			if (hit.transform.CompareTag ("EventObject")) {
				if (!flag) {
					flag = true;				
					eventTextObj.SetActive(flag);
				}

				if (Input.GetKeyDown (KeyCode.E)) {
					hit.transform.gameObject.SendMessage ("MakeEvent", SendMessageOptions.DontRequireReceiver);
				}

			} 
			else 
				if (flag) {
					flag = false;
					eventTextObj.SetActive(flag);
				}

		} 
		else 
			if (flag) {
				flag = false;
				eventTextObj.SetActive(flag);
			}
		


	}
}
