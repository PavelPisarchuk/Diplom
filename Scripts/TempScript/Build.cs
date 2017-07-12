using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Build : MonoBehaviour {
	[SerializeField] Transform Parent_Obj;
	[SerializeField] Transform wallPrefab_0;
	[SerializeField] Transform startPoint;
	[SerializeField] int[,] walls={

		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},


	};//18-9
	[SerializeField] Transform Type_1;
	[SerializeField] Transform Type_2;

	void Start()
	{
		
	}


	// Use this for initialization
	[ContextMenu("Generate")]
	void CreatePart()
	{
		for (int i = 0; i < 12; i++)
			for (int j = 0; j < 22; j++) {


				if (walls [i,j] != 0) {
					
					if (walls [i,j] == 1)
						Instantiate (wallPrefab_0, startPoint.position + new Vector3 (i*2, 0, j*2), Type_1.rotation, Parent_Obj);
					if (walls [i,j] == 2)
						Instantiate (wallPrefab_0, startPoint.position + new Vector3 (i*2, 0, j*2), Type_2.rotation, Parent_Obj);
					
				}


			}



	}



}
