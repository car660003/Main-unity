using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growing : MonoBehaviour {

	public Transform player_transform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance( player_transform.position, transform.position );
	}
}
