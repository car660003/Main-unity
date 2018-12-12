﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour {

	// Use this for initialization
	//public GameObject fatherFeild;
//	public GameObject CameraMain;

	void Start () {
//		CameraMain = Camera.main;
	}	
	// Update is called once per frame
	void Update () {


		//当前对象始终面向摄像机。
		//this.transform.LookAt(Camera.main.transform.position);
		//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - this.transform.position),0);

		this.transform.LookAt(VegetableMainMessange.mainCamera.transform.position);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(VegetableMainMessange.mainCamera.transform.position - this.transform.position),0);

	}
}
