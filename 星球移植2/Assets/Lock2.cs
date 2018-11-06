using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock2 : MonoBehaviour {

	public GameObject Lockbt;

	public void AcktiveLockbt(){
		if (cucumber.Count>=1) 
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
