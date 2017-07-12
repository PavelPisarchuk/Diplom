using UnityEngine;
using System.Collections;

public class StencilSwithcer : MonoBehaviour {

	
	[SerializeField] GameObject player;

	[SerializeField] GameObject[] hands;
	[SerializeField] GameObject otherStencilScreen;
	[SerializeField] GameObject otherStencilScreenObject;
	[SerializeField] string normalLayer;
	[SerializeField] string switchLayer;

	[SerializeField] string PlayerStencilLayer_0;
	[SerializeField] string PlayerStencilLayer_1;

	[Space(20)]
	[SerializeField] AudioSource switchAudioSource;
	[SerializeField] AudioSource on_offAudioSource;

	[SerializeField] AudioClip switchSound;
	[SerializeField] AudioClip can_useSound;
	[SerializeField] AudioClip off_onSound;
	[SerializeField] AudioClip abordSound;

	[Space(20)]
	[SerializeField] float swithcPauseTime;
	[SerializeField] float timeCallDown;
	float _timeCallDown;
	
	[Space(20)]
	[SerializeField] CameraEffects camEffects;
	
	bool canSwap=false;

	[SerializeField] GameObject usableObject;

	bool canSwapInPlace=true;
	bool swapNow;
	bool needSwapAfter;

	Pause _pause;


	[Space(20)]
	[SerializeField] Light flashLight;
	[SerializeField] LayerMask flashLightmask_0;
	[SerializeField] LayerMask flashLightmask_1;
	Level current_level;

	public Level CurrentLevel{
		set{ current_level = value; }
		get{ return  current_level; }
	}



	public bool UsabaleInPlace{
		get{ return canSwapInPlace; }
		set{ canSwapInPlace = value; }
	}

	public bool Usebale{
		get{ return canSwap; }
		set{ canSwap = value; }
	}

	int currentTime=0;

	[SerializeField] Material camPlane;
	[SerializeField] Material tvPlane;

	[SerializeField] CharacterController controller;
	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player");
		_pause = (Pause)FindObjectOfType (typeof(Pause));
		ToDefault ();
		ToDefault_CamMask ();
		//flashLight.enabled = false;
	}


	[ContextMenu("Swap")]
	void Swap(bool ignore)
	{
		if (CanSwap (canSwap,/*canSwapInPlace*/true,!swapNow) ) {			
		} 
		else 
			if (!ignore)
				return;
			else {
				if(swapNow)
					needSwapAfter = true;
			}

		swapNow = true;

		foreach(var x in hands)
			x.layer = LayerMask.NameToLayer (switchLayer);
		//otherStencilScreen.layer= LayerMask.NameToLayer (normalLayer);

		switchAudioSource.PlayOneShot (switchSound);
		player.SendMessage ("ClearMessage", SendMessageOptions.DontRequireReceiver);
		camEffects._needSwap = true;

		currentTime = currentTime == 0 ? 1 : 0;
		SwapAudioSourceTime (currentTime,true);
		StartCoroutine (WaitCoroutine());
	}

	void SwapAudioSourceTime(int time,bool onStart)
	{
		current_level.SwapSoundTime (time,onStart);
	}
	void SwapAudioSourceTimeFast(int time,bool onStart)
	{
		current_level.SwapSoundTimeFast (time,onStart);
	}

	void SwapAfterWait()
	{
		//tvPlane.SetInt ("_StencilVal",currentTime == 0 ? 0 : 1);
		//camPlane.SetInt ("_StencilVal",currentTime == 0 ? 1 : 0);
		//currentTime = currentTime == 0 ? 1 : 0;
		if (!canSwapInPlace) {
			on_offAudioSource.PlayOneShot (abordSound);
			ToDefault_CamMask ();
			swapNow = false;
			currentTime = currentTime == 0 ? 1 : 0;
			SwapAudioSourceTimeFast (currentTime,true);
			return;
		} else {
			if (currentTime == 0) {
				tvPlane.SetInt ("_StencilVal", 1);
				camPlane.SetInt ("_StencilVal", 0);
				flashLight.cullingMask = flashLightmask_0.value;
			} else {
				tvPlane.SetInt ("_StencilVal", 0);
				camPlane.SetInt ("_StencilVal", 1);
				flashLight.cullingMask = flashLightmask_1.value;
			}

			if (player.layer == LayerMask.NameToLayer (PlayerStencilLayer_1))
				player.layer = LayerMask.NameToLayer (PlayerStencilLayer_0);
			else
				player.layer = LayerMask.NameToLayer (PlayerStencilLayer_1);

			ToDefault_CamMask ();
			//controller.
			//Physics.IgnoreLayerCollision
			swapNow = false;
		}
	}


	bool CanSwap(bool bl1, bool bl2,bool bl3)
	{
		if (Time.time - timeCallDown > _timeCallDown && camEffects._needSwap==false && bl1 && bl2 && bl3) {
			_timeCallDown = Time.time + timeCallDown;
			return true;
		} else {
			if (!switchAudioSource.isPlaying) {
			}
				//switchAudioSource.PlayOneShot (can_useSound);
			return false;
		}

	}

	IEnumerator WaitCoroutine()
	{
			yield return new WaitForSeconds(swithcPauseTime);
			SwapAfterWait ();
	}


	public void ToDefaultWithEffect()
	{
		if (currentTime != 0)
			Swap (true);
	}
	public void ToDefaultWithOUTEffect()
	{
		if (currentTime != 0) {
			StopCoroutine (WaitCoroutine ());
			currentTime = 0;

			tvPlane.SetInt ("_StencilVal", 1);
			camPlane.SetInt ("_StencilVal", 0);
			flashLight.cullingMask = flashLightmask_0.value;

			player.layer = LayerMask.NameToLayer (PlayerStencilLayer_0);
			ToDefault_CamMask ();
			swapNow = false;
		}
	}

	void ToDefault()
	{
		tvPlane.SetInt ("_StencilVal",1);
		camPlane.SetInt ("_StencilVal",0);
		currentTime = 0;
		flashLight.cullingMask = flashLightmask_0.value;

	}

	void ToDefault_CamMask ()// для перемещения рук в другйо слой , нужно для того чтобы эффекты камеры работали на руки тоже
	{
		foreach(var x in hands)
			x.layer = LayerMask.NameToLayer (normalLayer);
		otherStencilScreen.layer= LayerMask.NameToLayer (switchLayer);
	}

	public void SwapTime()
	{
		Swap (false);
	}
	public void EnabledScreen()
	{
		if (canSwap && !_pause.Paused) {
			otherStencilScreenObject.SetActive (!otherStencilScreenObject.activeSelf);
			on_offAudioSource.PlayOneShot (off_onSound);
		}
	}
	// Update is called once per frame
	void Update () {		

		if (needSwapAfter && !swapNow) {
			needSwapAfter = false;
			Swap (false);
		}
		if(canSwap && canSwapInPlace && usableObject)
			usableObject.SetActive(true);
		else
			usableObject.SetActive(false);
		
	}
}
