using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harvestPumkin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider sickle){
		if(sickle.gameObject.name == "sickle"){
			pumpkin.pumpkinCount++;
			Destroy(this.gameObject);
		}

	}
}
