using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookopcl : MonoBehaviour {

		public GameObject Cube;

		public void ActiveCube(){
//		Cube.SetActive(true);
				if (!Cube.activeInHierarchy) 
				{
				Cube.SetActive (true);
				}
				else 
					{
				Cube.SetActive (false);
					}
			}


	}
