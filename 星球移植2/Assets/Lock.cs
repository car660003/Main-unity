using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour {

	public GameObject Lockbt;

	public void AcktiveLockbt(){
		//		Cube.SetActive(true);
		if (pumpkin.Count>=1) 
		{
			Lockbt.SetActive (true);
		}
		else 
		{
			Lockbt.SetActive (false);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AcktiveLockbt();

    }
}
