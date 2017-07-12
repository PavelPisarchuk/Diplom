using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
public class Level : MonoBehaviour {

	[SerializeField] Transform Portal_Position_Sender;
	[SerializeField] Transform Portal_Position_Reciver;
	[SerializeField] GameObject Lvl_Start_Obj;
	[SerializeField] Lvl_End lvlEnd;
	[SerializeField] Transform cam;
	[SerializeField] Image img;
	public GameObject Lvl_Start_Object{
		get{ return Lvl_Start_Obj; }
	}
	public Transform Sender_Pos{
		get{ return Portal_Position_Sender;}
	}
	public Transform Reciver_Pos{
		get{ return Portal_Position_Reciver;}
	}

	[SerializeField] Transform Player_Position;
	[SerializeField] GameObject[] WallsBehindPortal;
	[SerializeField] int _id;
	[SerializeField] bool _opened=false;

	SoundLevelManager lvlSoundManager;

	public bool Opened{
		get{ return _opened; }
		set{ _opened = value; 
			if (img) {
				img.enabled = _opened;
				Debug.Log (_id.ToString()+_opened.ToString());
			}
		}
	}

	public int Id{
		get{ return _id; }
		set{ _id = value; }
	}

	LevelManager lvlManager;


	FirstPersonController controller;

	[SerializeField] DefaultStateObject[] defaultStates;

	void Awake()
	{
		cam = Camera.main.transform;
		lvlManager = (LevelManager)FindObjectOfType (typeof(LevelManager));
		controller = (FirstPersonController)FindObjectOfType (typeof(FirstPersonController));
		lvlSoundManager = GetComponent<SoundLevelManager> ();
	}

	void Start()
	{


	}
	/*[ContextMenu("LevelEnd_Test")]
	public void LvlEnd()// сообщения для менеджера уровней о конце данного уровня
	{
		lvlManager.EndCurrentLevel (_id);
		Lvl_Start_Obj.SetActive (false);
	}*/
	public void ImgEneble()
	{
		if (img)
			img.enabled = true;
	}

	public void PlayerToPosition(GameObject plr)// перемещения героя в позицию начала уровня (если выбрал уровень из меню)
	{
		if (Vector3.Distance (plr.transform.position,Player_Position.position) < 50f)
			return;
		plr.transform.position = Player_Position.position;
		controller.CamReset (Player_Position);
	}
	IEnumerator WaitCoroutine(float time,GameObject plr)
	{
		yield return new WaitForSeconds(time);
		plr.transform.position = Player_Position.position;
		controller.CamReset (Player_Position);
	}


	public void SwapSoundTimeFast(int time,bool onStart)
	{
		lvlSoundManager.SwapFast (time, onStart);
	}
	public void SwapSoundTime(int time,bool onStart)
	{
		lvlSoundManager.Swap (time, onStart);
	}
	public void ToDefaultSound()
	{
		lvlSoundManager.ToDefault();
	}



	public void EnabledWallBehindPortal(bool active)// вкл/выкл стен за порталом 
	{
		for (int i = 0; i < WallsBehindPortal.Length; i++) {
			WallsBehindPortal [i].SetActive (active);
		}
	}


	public void ToDefaultLevelState()
	{
		for (int i = 0; i < defaultStates.Length; i++)
			defaultStates [i].ToDefault ();
		if(lvlEnd!=null)
			lvlEnd.Reset();
	}

}
