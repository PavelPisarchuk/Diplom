using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour {
	[SerializeField] float time;
	[SerializeField] Animator anim;
	[SerializeField] GameObject obj;
	[SerializeField] LayerMask mask_start;
	[SerializeField] LayerMask mask_cont;
	[SerializeField] Camera cam;
	// Use this for initialization
	void Start () {
		//obj.SetActive(false);
		//cam=GetComponent<Camera>();

		//cam.cullingMask=mask_start.value;
		//anim = GetComponent<Animator> ();
		//StartCoroutine (WaitCoroutine ());
	}
	


	void LoadScene()
	{
		cam.cullingMask=mask_cont.value;

		//SceneManager.LoadScene (1);
		//Debug.Log("ready");
		//obj.SetActive(true);
		//anim.SetBool ("Next",true);
	}

	IEnumerator WaitCoroutine()
	{
		yield return new WaitForSeconds(time);
		LoadScene ();
	}

}
