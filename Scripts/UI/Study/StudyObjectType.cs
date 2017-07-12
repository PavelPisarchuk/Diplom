using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyObjectType : MonoBehaviour {
	enum TypeOfStudyObj {menu,drag,shift};
	[SerializeField] TypeOfStudyObj tp;

	[Header("menu"),Space(10)]
	[SerializeField] GameObject obj;

	[Header("drag"),Space(10)]
	[SerializeField] GameObject obj_2;

	[Header("shift"),Space(10)]
	[SerializeField] GameObject obj_3;

	[SerializeField] float time;
	// Use this for initialization


	void MakeEvent()
	{
		if (tp == TypeOfStudyObj.menu) {
			Menu ();
		}
		if (tp == TypeOfStudyObj.drag) {
			Drag ();
		}
		if (tp == TypeOfStudyObj.shift) {
			Shift ();
		}
	}
	// Update is called once per frame
	void Shift () {
		offAll ();
		obj_3.SetActive (true);
		StartCoroutine (WaitCoroutine ());
	}

	void Drag () {
		offAll ();
		obj_2.SetActive (true);
		StartCoroutine (WaitCoroutine ());
	}

	void Menu () {
		offAll ();
		obj.SetActive (true);
		StartCoroutine (WaitCoroutine ());
	}


	void offAll()
	{
		obj.SetActive (false);
		obj_2.SetActive (false);
		obj_3.SetActive (false);
		StopCoroutine(WaitCoroutine());
	}

	IEnumerator WaitCoroutine()
	{
		yield return new WaitForSeconds(time);
		OffObj ();
	}

	void OffObj()
	{
		if (tp == TypeOfStudyObj.menu) {
			obj.SetActive (false);
		}
		if (tp == TypeOfStudyObj.drag) {
			obj_2.SetActive (false);
		}
		if (tp == TypeOfStudyObj.shift) {
			obj_3.SetActive (false);
		}

	}

}
