using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStore : MonoBehaviour {


	public GameObject store;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider player){
		if (player.gameObject.name == "Camera (eye)") {
			store.SetActive(true);
		}
	}
	void OnTriggerExit(Collider player){
		if (player.gameObject.name == "Camera (eye)") {
			store.SetActive(false);
		}
	}
}
