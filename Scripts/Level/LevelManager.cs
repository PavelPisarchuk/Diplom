using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour {
	[SerializeField] List<Level> Levels;

	[SerializeField] GameObject Portal_Sender;
	[SerializeField] GameObject Portal_Reciver;
	[SerializeField] GameObject Camera_1;
	[SerializeField] GameObject Camera_2;

	[SerializeField] int lastOpenedLevel_id=0;
	[SerializeField] HandController hands;
	[SerializeField] StencilSwithcer swithc;
	//bool waitStartLevel;
	Transform Player;

	[SerializeField] int current_level_id=0;
	int last_current_level=1;

	public int Current_level
	{
		get{ return current_level_id; }
		set{ current_level_id = value; }
	}

	[SerializeField] int to_level;

	[ContextMenu("To_Level")]
	void TO_L()
	{
		ToLevel (to_level);
	}

	[ContextMenu("Clear")]
	void Clear()
	{
		PlayerPrefs.DeleteAll ();
	}

	void Awake()
	{
		//Levels = SortedList (Levels);
		Read ();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		hands = GameObject.FindGameObjectWithTag ("Player").GetComponent<HandController> ();
		swithc = (StencilSwithcer)FindObjectOfType(typeof(StencilSwithcer));
		hands.OffLeftHand();
		hands.OffRightHand();
	}

	List<Level> SortedList(List<Level> lvls)
	{
		return lvls.OrderBy (x => x.Id).ToList();
	}

	bool HaveIdCollision()
	{
		for (int i = 0; i < Levels.Count - 1; i++) {
			if (Levels [i].Id == Levels [i + 1].Id)
				return true;
		}
		return false;
	}

	[ContextMenu("Save")]
	void Save()
	{
		//if (PlayerPrefs.GetInt ("LastOpenedLevel", 0)>=lastOpenedLevel_id)
		//	return;
		lastOpenedLevel_id = Levels.FindLast (x => x.Opened == true).Id;
		Levels.Find (x => x.Id == lastOpenedLevel_id ).ImgEneble ();
		PlayerPrefs.SetInt("LastOpenedLevel",lastOpenedLevel_id);
		PlayerPrefs.Save ();
	}
	[ContextMenu("Read")]
	void Read()
	{
		Levels = SortedList (Levels);
		if(HaveIdCollision())
			throw new Exception("Have duplicate Level ID ");
		lastOpenedLevel_id = PlayerPrefs.GetInt("LastOpenedLevel",1);
		SetLevelActive ();
	}
	void SetLevelActive()
	{
		foreach (var x in Levels) {			
			if (x.Id <= lastOpenedLevel_id)
				x.Opened = true;
		}

	}

/*	public void StartLevel()
	{
		waitStartLevel = false;
		Debug.Log ("DSADSADASDAS");
	}*/

	public void StartNewLvel(int id)
	{
		PortalActiveController (false);

		if(current_level_id!=0 )
			Levels.Find (x => x.Id == current_level_id).gameObject.SetActive(false);


		swithc.CurrentLevel = Levels.Find (x => x.Id == id);
		current_level_id = id;
	}

	public void EndCurrentLevel(int id)// конец ID уровня 
	{
		if (id == Levels.FindLast (x=>x).Id)
			return;
		Levels.Find (x => x.Id == id+1).Opened = true;
		Levels.Find (x => x.Id == id).ToDefaultSound ();
		Save ();
		ToLevel (id + 1);
		//PortalActiveController (true);
	}


	public void ToLevelFromMenu(int id)
	{
		//if (id > lastOpenedLevel_id )
		//	return;
		/*if(current_level_id!=0)
			Levels.Find (x => x.Id == current_level_id).gameObject.SetActive(false);

		if(last_current_level!=0)
			Levels.Find (x => x.Id == last_current_level).gameObject.SetActive(false);
		last_current_level = id;
		*/
		if (!Levels.Find (x => x.Id == id).Opened)
			return;
		foreach (var x in Levels) {
			if (x.Id != 0)
				x.gameObject.SetActive (false);
		}

		//waitStartLevel = false;
		current_level_id = 0;


		Level lvl = Levels.Find (x => x.Id == id);
		lvl.ToDefaultLevelState ();
		lvl.gameObject.SetActive (true);

		Level currentLevel = Levels.Find (x => x.Id == 0);
		currentLevel.Lvl_Start_Object.SetActive (false);
		lvl.Lvl_Start_Object.SetActive (true);

		//currentLevel.PlayerToPosition (Player.gameObject);
		Levels.Find (x => x.Id == 0).PlayerToPosition (Player.gameObject);

		PortalToPostition( currentLevel.Sender_Pos ,lvl.Reciver_Pos);
		PortalActiveController (true);
		lvl.EnabledWallBehindPortal (false);



		//swithc.CurrentLevel = lvl;
		swithc.ToDefaultWithOUTEffect ();
		hands.OffLeftHand();
		hands.OffRightHand();
	}


	void ToLevel(int id)// перемещения порталов в позицию нового уровня и отключение стен сзади портала который находится в новом уровне (EnabledWallBehindPortal)
	{
		//if (waitStartLevel)
	//		return;
		if (id > Levels.Last ().Id)
			return;
		Level lvl = Levels.Find (x => x.Id == id);
		//if (!lvl.Opened)
		//	return;
		lvl.ToDefaultLevelState ();
		lvl.gameObject.SetActive (true);

		Level currentLevel = Levels.Find (x => x.Id == current_level_id);
		currentLevel.Lvl_Start_Object.SetActive (false);
		lvl.Lvl_Start_Object.SetActive (true);

		PortalToPostition( currentLevel.Sender_Pos ,lvl.Reciver_Pos);
		PortalActiveController (true);
		lvl.EnabledWallBehindPortal (false);

		hands.OffLeftHand();
		hands.OffRightHand();

		//swithc.CurrentLevel = lvl;
		swithc.ToDefaultWithEffect ();
	}

	public void PortalActiveController(bool active)//true-когда конец уровня и переход на новый  false- когда прошёл через порта и начал новый уровень
	{
		//Portal_Sender.SetActive (active);
		//Portal_Reciver.SetActive (active);
		if (active) {
			Camera_1.SetActive (true);
			Camera_2.SetActive (true);
			Portal_Sender.BroadcastMessage ("On_Portal", SendMessageOptions.DontRequireReceiver);
			Portal_Reciver.BroadcastMessage ("On_Portal", SendMessageOptions.DontRequireReceiver);
		} else {
			Portal_Sender.BroadcastMessage ("Off_Portal", SendMessageOptions.DontRequireReceiver);
			Portal_Reciver.BroadcastMessage ("Off_Portal", SendMessageOptions.DontRequireReceiver);	
			Camera_1.SetActive (false);
			Camera_2.SetActive (false);

		}

	}


	public void PortalToPostition(Transform sender, Transform reciver)
	{
		Portal_Sender.transform.position = sender.position;
		//Portal_Sender.transform.rotation = sender.rotation;

		Portal_Reciver.transform.position = reciver.position;
		//Portal_Reciver.transform.rotation = reciver.rotation;

	}


}
