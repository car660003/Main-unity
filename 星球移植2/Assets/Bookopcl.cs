using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookopcl : MonoBehaviour {

		public GameObject Book;

		public void AcktiveBook(){
//		Cube.SetActive(true);
		if (!Book.activeInHierarchy) 
			{
			Book.SetActive (true);
			}
		else 
			{
			Book.SetActive (false);
			}
		}


	}
