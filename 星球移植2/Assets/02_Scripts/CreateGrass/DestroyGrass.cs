using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGrass : MonoBehaviour {

	public int number;//存入自己的編號
	public int hitNum;

	public bool ch01=false;
	public bool ch02=false;

	public CreateGrass grassCreatePoint;
	public Transform grass;

	// Use this for initialization
	void Start () {
		grassCreatePoint = gameObject.GetComponentInParent<CreateGrass> ();
		hitNum = Random.Range (1, 6);
	}
	
	// Update is called once per frame
	void Update () {
		grass = gameObject.GetComponentInChildren<Transform> ();
	}

	void OnTriggerEnter(Collider sickle){
		if(sickle.tag == "sickle"){
			hitNum--;
			if(grassCreatePoint.checkGrass[number]==true&&hitNum<=0){
				hitNum = Random.Range (1, 6);
				grassCreatePoint.checkGrass [number] = false;
				grassCreatePoint.nowGrass--;
				Destroy (transform.GetChild (0).gameObject);
			}
		}
	}
}
