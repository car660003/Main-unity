using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSale : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Sale(){
		
		if(gameObject.tag=="cucumber"){
			VegetableMainMessange.money += cucumber.salePrice;
			cucumber.Count -= 1;
		}
		if(gameObject.tag=="eggplant"){
			VegetableMainMessange.money += eggplant.salePrice;
			eggplant.Count -= 1;
		}
		if(gameObject.tag=="carrot"){
			VegetableMainMessange.money += carrot.salePrice;
			carrot.Count -= 1;
		}
		if(gameObject.tag=="pumpkin"){
			VegetableMainMessange.money += pumpkin.salePrice;
			pumpkin.Count -= 1;
		}
		if(gameObject.tag=="tomato"){
			VegetableMainMessange.money += tomato.salePrice;
			tomato.Count -= 1;
		}
	}
}
