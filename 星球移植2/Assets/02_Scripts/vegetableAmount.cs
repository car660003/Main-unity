using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class vegetableAmount : MonoBehaviour {


	public Text text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "南瓜收成："+pumpkin.Count.ToString ()+"個";
	}
}
