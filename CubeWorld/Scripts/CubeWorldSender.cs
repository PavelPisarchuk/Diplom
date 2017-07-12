using UnityEngine;
using System.Collections;
using System;
public class CubeWorldSender : MonoBehaviour {
	[SerializeField] Material camPlane;

	[SerializeField] GameObject Self_PlaneObj;
	[SerializeField] GameObject Stn0_PlaneObj;
	[SerializeField] GameObject Borders;
	[SerializeField] GameObject[] OtherWorlds;
	[SerializeField] GameObject[] CurrentWorlds;


	[SerializeField] int stencil;
	[SerializeField] string default_layer;
	[SerializeField] string current_layer;

	bool playerOverlapping = false;

	[SerializeField] Transform player;
	[SerializeField] float currentDot;
	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
		default_layer= LayerMask.LayerToName(player.gameObject.layer);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.C)) {
			Time.timeScale = 0.1f;
		}
		if (Input.GetKeyDown (KeyCode.V)) {
			Time.timeScale = 1.0f;
		}

		if (playerOverlapping) {
			currentDot = Vector3.Dot(transform.up, player.position - transform.position);
	
			if (currentDot < 0) {
				Inside ();
			} else 
				if (currentDot > 0)
					Out ();
		}


	}

	void Inside()
	{
		Stn0_PlaneObj.SetActive (true);
		Self_PlaneObj.SetActive (false);

		camPlane.SetInt ("_StencilVal", stencil);
		player.gameObject.layer = LayerMask.NameToLayer (current_layer);
		gameObject.layer = player.gameObject.layer;
		Borders.SetActive (false);
		WorldEneblaCOntroller (false,OtherWorlds);
		WorldEneblaCOntroller (true,CurrentWorlds);

	}

	void WorldEneblaCOntroller(bool bl, GameObject[] objs)
	{
		for (int i = 0; i < objs.Length; i++) {
			objs [i].SetActive (bl);
		}

	}

	void Out()
	{
		Stn0_PlaneObj.SetActive (false);
		Self_PlaneObj.SetActive (true);

		camPlane.SetInt ("_StencilVal", 0);
		player.gameObject.layer = LayerMask.NameToLayer (default_layer);
		gameObject.layer = player.gameObject.layer;
		Borders.SetActive (true);
		WorldEneblaCOntroller (true,OtherWorlds);
		WorldEneblaCOntroller (false,CurrentWorlds);

	}




	void OnTriggerEnter(Collider other) {

		if (other.CompareTag("Player"))
		{
			playerOverlapping = true;
		}	
	}

	void OnTriggerExit(Collider other) {

		if (other.CompareTag("Player"))
		{
			playerOverlapping = false;
		}
	}
	

	
}
