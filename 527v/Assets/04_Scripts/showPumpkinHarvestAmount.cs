using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showPumpkinHarvestAmount : MonoBehaviour {

	// Use this for initialization
	public Text pumpkinHarvestAmount_Text;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pumpkinHarvestAmount_Text.text = "南瓜收成" + pumpkin.pumpkinHarvestAmount + "個";
	}
}
