using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TextureSetup : MonoBehaviour {

	[SerializeField] Camera camera1;
	[SerializeField] Camera camera2;

	[SerializeField] Material camera1Mat;
	[SerializeField] Material camera2Mat;

	[SerializeField] int antiAliasingLevel;

    // When game starts remove current camera textures and set new textures with the dimensions of the players screen
    void Awake()
    {

		antiAliasingLevel = QualitySettings.antiAliasing;

        if (camera1.targetTexture != null)
        {
            camera1.targetTexture.Release();
        }

        camera1.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera1Mat.mainTexture = camera1.targetTexture;

        if (camera2.targetTexture != null)
        {
            camera2.targetTexture.Release();
        }
        camera2.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera2Mat.mainTexture = camera2.targetTexture;
        


		if (antiAliasingLevel == 0 || antiAliasingLevel == 1) {

		} else {
			camera2.targetTexture.antiAliasing = antiAliasingLevel;
			camera1.targetTexture.antiAliasing = antiAliasingLevel;
		}




    }



}
