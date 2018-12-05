using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMesh : MonoBehaviour {

	public MeshRenderer thisMesh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		thisMesh = gameObject.GetComponent<MeshRenderer> ();
		if(thisMesh){
			thisMesh.enabled = false;
		}
	}

}
