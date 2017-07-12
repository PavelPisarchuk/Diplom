using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraEffects : MonoBehaviour {

	[SerializeField] bool needSwap=false;
	public bool _needSwap{
		get{ return needSwap; }
		set{ needSwap = value;
			//clrCor.enabled = true;	
		}
	}
	[SerializeField] float speed=5;
	[Space(20)]

	[SerializeField] ColorCorrectionCurves clrCor;
	float toSaturation;
	bool clrFast;

	[Space(20)]
	[SerializeField] Vortex twirl;
	int ToAngle;
	[SerializeField] int MaxAngle;


	[Space(20)]
	[SerializeField] Fisheye fisheye;
	float ToStrY;
	[SerializeField] float MaxToStrY;


	[Space(20)]
	[SerializeField] VignetteAndChromaticAberration VigCh;
	float ToChromo;
	[SerializeField] float MaxToChromo;
	float ToVignet;
	[SerializeField] float MaxToVignet;



	[Space(20)]
	[SerializeField] Camera cam;
	float ToView;
	float startView;
	[SerializeField] float MaxToView;


	bool faster=true;
	bool goBack=false;

	public bool GoBack{
		get{ return goBack; }
	}

	public float Zero_One
	{
		get{return clrCor.saturation;}
	}
	// Use this for initialization
	void Awake()
	{
		Application.targetFrameRate = 60;
		ToAngle = MaxAngle;
		ToStrY = MaxToStrY;
		ToChromo = MaxToChromo;
		ToVignet = MaxToVignet;
		ToView = MaxToView;
		startView = cam.fieldOfView;
		toSaturation = 0;
	}


	void Swap()
	{
		clrCor.saturation =Mathf.Clamp( Mathf.Lerp (clrCor.saturation, toSaturation, Time.deltaTime * speed * (faster?1:3)),0,1);        


		//twirl.angle = Mathf.Clamp(Mathf.Lerp (twirl.angle, ToAngle, Time.deltaTime * speed * (faster?1:3) ),0,MaxAngle);
		//if (twirl.angle > MaxAngle - 5) {
			//ToAngle = -1;
	//		faster = false;
	//		ToStrY = -0.1f;// 0;
			//ToChromo = 0;
			//ToVignet = 0.1f;
			//ToView = startView-1;
			//toSaturation = 1.1f;
	//	}

		fisheye.strengthY = Mathf.Clamp( Mathf.Lerp (fisheye.strengthY, ToStrY, Time.deltaTime * speed * (faster?1:3))  , 0, MaxToStrY  );

		if (clrCor.saturation < 0.1f) {
			toSaturation = 1.1f;
			ToStrY = -0.1f;
			faster = false;
			goBack = true;
		}

		//if (fisheye.strengthY > MaxToStrY-0.1f)
		//	ToStrY = 0;

	//	VigCh.chromaticAberration = Mathf.Lerp (VigCh.chromaticAberration, ToChromo, Time.deltaTime * speed * (faster?1:3));
		//if (VigCh.chromaticAberration > MaxToChromo-1f)
		//	ToChromo = 0;

	//	VigCh.intensity = Mathf.Lerp (VigCh.intensity, ToVignet, Time.deltaTime * speed * (faster?1:3));
	//	if (VigCh.intensity > MaxToVignet-0.1f)
	//		ToVignet = 0.1f;

	//	cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, ToView, Time.deltaTime * speed *(faster?1:5) );
	//	if (cam.fieldOfView > MaxToView - 5)
	//		ToView = startView-1;


		if (fisheye.strengthY==0 && clrCor.saturation==1) {
			EndSwap ();
		}


	}

	void EndSwap()
	{
	//	twirl.angle = 0;
	//	ToAngle = MaxAngle;

		fisheye.strengthY = 0;
		ToStrY = MaxToStrY;
		goBack = false;

	//	VigCh.chromaticAberration = 0;
//		VigCh.intensity = 0.1f;
	//	ToChromo = MaxToChromo;
	//	ToVignet = MaxToVignet;

	//	cam.fieldOfView = startView;
	//	ToView = MaxToView;

		clrCor.saturation = 1;
		toSaturation = 0;

		faster=true;

		needSwap = false;
		//clrCor.enabled = false;
	}


	// Update is called once per frame
	void Update () {
		
		if (needSwap)
			Swap ();
		
	}




}
