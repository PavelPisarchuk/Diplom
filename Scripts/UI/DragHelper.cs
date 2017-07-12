using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragHelper : MonoBehaviour {
	[SerializeField] Text text; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			if (text.enabled == true)
				text.enabled = false;
		}
	}

	void MakeEvent()
	{
		text.enabled = true;
	}

}
