using UnityEngine;
using System.Collections;

public class CamStencil : MonoBehaviour {

	[SerializeField] Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		Graphics.Blit(src, dest, mat);
	}
}
