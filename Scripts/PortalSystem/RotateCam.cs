using UnityEngine;
using System.Collections;

public class RotateCam : MonoBehaviour {
	[SerializeField] Camera cam;

	[SerializeField] Transform portal1;
	[SerializeField] Camera portal1Cam;

	[SerializeField] Transform portal2;
	[SerializeField] Camera portal2Cam;


	// Use this for initialization
	void Start () {
		// Camera.main.depthTextureMode = DepthTextureMode.Depth;
	}

	// Update is called once per frame
	void LateUpdate () {
		//sky.position = cam.transform.position;

		Quaternion q = Quaternion.FromToRotation (-portal1.up, cam.transform.forward);
		portal1Cam.transform.position = portal2.position + (cam.transform.position - portal1.position);
		portal1Cam.transform.LookAt (portal1Cam.transform.position + q * portal2.up, portal2.transform.forward);
		//portal1Cam.nearClipPlane = (portal1Cam.transform.position - portal2.position).magnitude - 0.3f;

		q = Quaternion.FromToRotation (-portal2.up, cam.transform.forward);
		portal2Cam.transform.position = portal1.position + (cam.transform.position - portal2.position);
		portal2Cam.transform.LookAt (portal2Cam.transform.position + q * portal1.up, portal1.transform.forward);
		//portal2Cam.nearClipPlane = (portal2Cam.transform.position - portal1.position).magnitude - 0.3f;
	}

}