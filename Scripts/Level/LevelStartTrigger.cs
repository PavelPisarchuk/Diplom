using UnityEngine;
using System.Collections;

public class LevelStartTrigger : MonoBehaviour {
	[SerializeField] LevelManager lvlManager;
	[SerializeField] HandController hands;
	[SerializeField] Level lvl;

	enum HandType {Right, Left, Right_Left};

	[Header("Left-Time  Right-Drag")]
	[SerializeField] HandType handsType;

	void Awake()
	{
		lvlManager = (LevelManager)FindObjectOfType (typeof(LevelManager));
		hands = GameObject.FindGameObjectWithTag ("Player").GetComponent<HandController> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			if (lvlManager.Current_level == lvl.Id)
				return;
			//lvlManager.PortalActiveController (false);
			lvl.EnabledWallBehindPortal (true);
			//lvlManager.of
			//lvlManager.Current_level = lvl.Id;
			lvlManager.StartNewLvel(lvl.Id);
			//if(PlayerPrefs.GetInt ("LastOpenedLevel")>2)				
				HandEnabled ();	

		}
	}

	public void HandEnabled()
	{
			if (handsType == HandType.Right_Left) {
				hands.OnLeftHand ();
				hands.OnRightHand ();
			}
			else
				if (handsType == HandType.Left)
					hands.OnLeftHand ();
				else
					if(handsType == HandType.Right)
						hands.OnRightHand ();	
	}


}
