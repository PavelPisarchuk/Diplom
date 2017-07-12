using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
	[SerializeField] GameObject PausedObj;
	[SerializeField] GameObject nonPausedObj;

	[SerializeField] HandSmouth handSmth;
	//[SerializeField] HandController hand;
	[SerializeField] FirstPersonController controller;
	[SerializeField] DragObjects drag;
	[SerializeField] AnimController anims;
	[SerializeField] StencilSwithcer swithcer;

	[SerializeField] CursorLockMode pauseMode;
	[SerializeField] CursorLockMode	playeMode;


	//[SerializeField] GameObject levelSelectorObj;
	bool levelEnabled=false;
	bool paused = false;

	//[SerializeField] Animator MainAnim;
	//[SerializeField] Animator LevelSelectorAnim;



	[Space(20)]
	[SerializeField] Color pressedColor;
	[SerializeField] Color normalColor;

	[SerializeField] Image GamePanel;
	[SerializeField] Image LevelPanel;

	[SerializeField] GameObject GamePanelobj;
	[SerializeField] GameObject LevelPanelobj;

	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip swithcSound;

	[SerializeField] GameObject LoadObject;
	//bool enabled

	public bool Paused{
		get{return paused;}
	}
	// Use this for initialization
	void Awake () {
		controller = (FirstPersonController)FindObjectOfType (typeof(FirstPersonController));
		drag = (DragObjects)FindObjectOfType (typeof(DragObjects));
		swithcer = (StencilSwithcer)FindObjectOfType (typeof(StencilSwithcer));
		handSmth = (HandSmouth)FindObjectOfType (typeof(HandSmouth));
		anims = (AnimController)FindObjectOfType (typeof(AnimController));
		//hand = (HandController)FindObjectOfType (typeof(HandController));
		Cursor.lockState = playeMode;
		Cursor.visible = (CursorLockMode.Locked != playeMode);


		GamePanel.color = pressedColor;
		LevelPanel.color = normalColor;
		GamePanelobj.SetActive (true);
		LevelPanelobj.SetActive (false);
	}

	public void GameP()
	{
		_as.PlayOneShot (swithcSound);

		LevelPanelobj.SetActive (false);
		GamePanelobj.SetActive (true);

		LevelPanel.color = normalColor;
		GamePanel.color = pressedColor;
	}

	public void LevelP()
	{
		_as.PlayOneShot (swithcSound);

		LevelPanelobj.SetActive (true);
		GamePanelobj.SetActive (false);

		LevelPanel.color = pressedColor;
		GamePanel.color = normalColor;
	}


	public void PauseEvent(bool bl=false)
	{		
		//MainAnim.Play("On", -1, 0f);

		PausedObj.SetActive (bl);
		nonPausedObj.SetActive (!bl);
		controller.FreezeMovement(bl);
		controller.enabled = !bl;
		//drag.enabled = !bl;
		handSmth.enabled = !bl;
		swithcer.UsabaleInPlace = !bl;

		if (!bl) {
			Cursor.lockState = pauseMode;
			Cursor.visible = (CursorLockMode.Locked != pauseMode);
		} else {
			Cursor.lockState = playeMode;
			Cursor.visible = (CursorLockMode.Locked != playeMode);
		}


		paused = bl;
		//if (!paused)
	//		LevelSelector (false);
	}

/*	public void LevelSelector()
	{
		levelEnabled = !levelEnabled;
		levelSelectorObj.SetActive (levelEnabled);
		LevelSelectorAnim.Play("On", -1, 0f);
	}

	public void LevelSelector(bool bl)
	{
		levelSelectorObj.SetActive (bl);
		levelEnabled = bl;
	}*/

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			PauseEvent (!paused);
	}


	public void ToMainMenu()
	{
		LoadObject.SetActive (true);
		SceneManager.LoadScene (0);
	}

}
