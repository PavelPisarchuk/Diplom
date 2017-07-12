using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSize : MonoBehaviour {
	[SerializeField] Text[] texts;
	[SerializeField] Text mainText;
	// Use this for initialization
	void Awake () {
		Resize ();
	}

	void Resize()
	{
		for (int i = 0; i < texts.Length; i++)
			texts [i].fontSize = mainText.fontSize;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
