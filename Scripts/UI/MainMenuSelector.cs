using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuSelector : MonoBehaviour {
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip swithcSound;

	[SerializeField] GameObject PauseObj;

	[SerializeField] Image GamePanel;
	[SerializeField] Image SettingsPanel;
	[SerializeField] Image ExitPanel;

	[Space(20)]
	[SerializeField] GameObject GamePanelobj;
	[SerializeField] GameObject SettingsPanelobj;
	[SerializeField] GameObject ExitPanelobj;

	[Space(20)]
	[SerializeField] Color pressedColor;
	[SerializeField] Color normalColor;

	[Space(20)]
	[SerializeField] Transform cam;
	[SerializeField] Transform targetRotation;
	[SerializeField] float rotateCamSpeed;

	[Space(20)]
	[SerializeField] GameObject prologObj;
	[SerializeField] GameObject contObj;
	[SerializeField] GameObject startObj;


	[System.Serializable]
	public struct DropDownSettings
	{
		public int index;
		public string options;
		public int value_1;
		public int value_2;
		public float value_3;

		public DropDownSettings(int index, string options, int value_1, int value_2, float value_3)
		{
			this.index = index;
			this.options = options;
			this.value_1 = value_1;
			this.value_2 = value_2;
			this.value_3 = value_3;
		}
	}

	[SerializeField] Toggle v_sync;
	[Space(20)]
	[SerializeField] List<DropDownSettings> resolutions;
	[SerializeField] Dropdown dropDownResol;

	[Space(20)]
	[SerializeField] List<DropDownSettings> antialaisings;
	[SerializeField] Dropdown dropDownAntialaisings;

	[Space(20)]
	[SerializeField] List<DropDownSettings> textures;
	[SerializeField] Dropdown dropDownTextures;

	[Space(20)]
	[SerializeField] List<DropDownSettings> shadows;
	[SerializeField] Dropdown dropDownShadow;


	[SerializeField] ShadowQuality sh;
	[SerializeField] ShadowResolution sh_res;
	//[SerializeField] DropDownSettings 

	// Use this for initialization
	void Awake () {
		SettingsPanel.color = normalColor;
		ExitPanel.color = normalColor;
		GamePanel.color = pressedColor;

		startObj.SetActive (false);
		contObj.SetActive (false);
		prologObj.SetActive (false);

		if (PlayerPrefs.GetString ("FirstTime", "true") == "true") {
			startObj.SetActive (true);
		} else {
			contObj.SetActive (true);
			prologObj.SetActive (true);
		}



		SetResolutionDropDown ();
		SetAntialaisingDropDown ();
		SetTextureDropDown ();
		SetLightDropDown ();


		ReadSettings ();
	}

	void SetLightDropDown()
	{
		shadows.Add (new DropDownSettings(0,"Выкл",0,0,8));
		shadows.Add (new DropDownSettings(1,"Оч.низкие",1,0,8));
		shadows.Add (new DropDownSettings(2,"Низкие",1,1,8));
		shadows.Add (new DropDownSettings(3,"Средние",1,2,8));
		shadows.Add (new DropDownSettings(4,"Высокие",1,3,8));

		List<string> DropDownStrings=new List<string>();
		foreach (var x in shadows)
			DropDownStrings.Add (x.options);

		dropDownShadow.ClearOptions ();
		dropDownShadow.AddOptions (DropDownStrings);
	}

	void SetTextureDropDown()
	{
		textures.Add (new DropDownSettings(0,"Очень низкие",3,8,8));
		textures.Add (new DropDownSettings(1,"Низкие",2,4,4));
		textures.Add (new DropDownSettings(2,"Средние",1,2,2));
		textures.Add (new DropDownSettings(3,"Высокие",0,1,1));

		List<string> DropDownStrings=new List<string>();
		foreach (var x in textures)
			DropDownStrings.Add (x.options);

		dropDownTextures.ClearOptions ();
		dropDownTextures.AddOptions (DropDownStrings);
	}

	void SetAntialaisingDropDown()
	{
		antialaisings.Add (new DropDownSettings(0,"Выкл",0,8,8));
		antialaisings.Add (new DropDownSettings(1,"x2",2,4,4));
		antialaisings.Add (new DropDownSettings(2,"x4",4,2,2));
		antialaisings.Add (new DropDownSettings(3,"x8",8,0,0));

		List<string> DropDownStrings=new List<string>();
		foreach (var x in antialaisings)
			DropDownStrings.Add (x.options);

		dropDownAntialaisings.ClearOptions ();
		dropDownAntialaisings.AddOptions (DropDownStrings);
	}

	void SetResolutionDropDown()
	{
		List<string> DropDownStrings=new List<string>();
		for (int i = 0, index=0; i < Screen.resolutions.Length; i++) {
			Resolution resol = Screen.resolutions [i];
			string str = resol.width.ToString () + "x" + resol.height.ToString ();
			var fndElement = resolutions.Find (t => t.options == str);
			if ( fndElement.options==null  ) {
				DropDownSettings item = new DropDownSettings (index, str, resol.width, resol.height, resol.refreshRate);
				resolutions.Add (item);
				DropDownStrings.Add (str);
				index++;
			}
		}
		dropDownResol.ClearOptions ();
		dropDownResol.AddOptions (DropDownStrings);
	}

	// Update is called once per frame
	void Update () {
		//RotateCam ();	
	}
	void RotateCam()
	{
		cam.rotation = Quaternion.Lerp (cam.rotation, targetRotation.rotation, Time.deltaTime * rotateCamSpeed);
	}

	public void GameP()
	{
		_as.PlayOneShot (swithcSound);

		SettingsPanelobj.SetActive (false);
		ExitPanelobj.SetActive (false);
		GamePanelobj.SetActive (true);

		SettingsPanel.color = normalColor;
		ExitPanel.color = normalColor;
		GamePanel.color = pressedColor;
	}

	public void ExitP()
	{
		_as.PlayOneShot (swithcSound);

		SettingsPanelobj.SetActive (false);
		ExitPanelobj.SetActive (true);
		GamePanelobj.SetActive (false);

		SettingsPanel.color = normalColor;
		ExitPanel.color = pressedColor;
		GamePanel.color = normalColor;
	}

	public void SettingsP()
	{
		_as.PlayOneShot (swithcSound);

		SettingsPanelobj.SetActive (true);
		ExitPanelobj.SetActive (false);
		GamePanelobj.SetActive (false);

		SettingsPanel.color = pressedColor;
		ExitPanel.color = normalColor;
		GamePanel.color = normalColor;
	}


	public void StartGame()
	{
		if (PlayerPrefs.GetString ("FirstTime", "true") == "true") {
			PauseObj.SetActive (true);
			SceneManager.LoadScene (1);
			//StartProlog ();
		} else {
			PauseObj.SetActive (true);
			SceneManager.LoadScene (1);
		}

	}

	[ContextMenu("TEmpNonFirsTime")]
	void das()
	{
		PlayerPrefs.SetString ("FirstTime", "false");
		PlayerPrefs.Save ();
	}
	public void StartProlog()
	{
		//PlayerPrefs.SetString ("FirstTime", "false");
	}

	public void PauseObjActive()
	{
		
	}

	public void ConfrimSettings()
	{

		var resol = resolutions.Find (t => t.index == dropDownResol.value);
		Screen.SetResolution (resol.value_1, resol.value_2, true);

		var antialias = antialaisings.Find (t => t.index == dropDownAntialaisings.value);
		QualitySettings.antiAliasing = antialias.value_1;

		var texture = textures.Find (t => t.index == dropDownTextures.value);
		QualitySettings.masterTextureLimit = texture.value_1;

		QualitySettings.vSyncCount = v_sync.isOn ? 1 : 0;

		var shadow = shadows.Find (t => t.index == dropDownShadow.value);
		ShadowQuality sh_q=new ShadowQuality();
		ShadowResolution sh_resol=new ShadowResolution();
		if (shadow.value_1 == 0) {
			sh_q = ShadowQuality.Disable;
		} else 
			if (shadow.value_1 == 1) {
				sh_q = ShadowQuality.All;
				switch( shadow.value_2 )   
				{  
					case 0:  
						sh_resol=ShadowResolution.Low; 
						break;
					case 1:  
						sh_resol=ShadowResolution.Medium;
						break;
					case 2:
						sh_resol=ShadowResolution.High; 
						break;
					case 3:
						sh_resol=ShadowResolution.VeryHigh;
						break;
					default :  
						sh_resol=ShadowResolution.Medium; 
						break;
				}  

			}
		QualitySettings.shadows = sh_q;
		QualitySettings.shadowResolution = sh_resol;


		SaveSettings (resol.index,antialias.index,texture.index,QualitySettings.vSyncCount,shadow.index);
	}


	void SaveSettings(int resoltype,int antiliastype,int texturetype,int vsyncvalue,int shadowtype)
	{
		PlayerPrefs.SetInt ("Resolution", resoltype);
		PlayerPrefs.SetInt ("Antiliasing", antiliastype);
		PlayerPrefs.SetInt ("Texture",texturetype);
		PlayerPrefs.SetInt ("Vsync",vsyncvalue);
		PlayerPrefs.SetInt ("Shadow",shadowtype);
		PlayerPrefs.Save ();
	}

	void ReadSettings()
	{
		dropDownResol.value = PlayerPrefs.GetInt ("Resolution",0 );
		dropDownAntialaisings.value = PlayerPrefs.GetInt ("Antiliasing",0);
		dropDownTextures.value = PlayerPrefs.GetInt ("Texture",1);
		v_sync.isOn = PlayerPrefs.GetInt ("Vsync",0) == 1 ? true : false;
		dropDownShadow.value = PlayerPrefs.GetInt ("Shadow",3);
		ConfrimSettings ();
	}


	public void Exit()
	{
		PauseObj.SetActive (true);
		PlayerPrefs.Save ();
		Application.Quit ();
	}

}
