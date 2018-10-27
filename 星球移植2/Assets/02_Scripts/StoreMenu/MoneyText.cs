using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour {

	public Text money_Text;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		money_Text.text = VegetableMainMessange.money.ToString();
		//money_Text.text = cucumber.seedAmount.ToString();
	}
}
