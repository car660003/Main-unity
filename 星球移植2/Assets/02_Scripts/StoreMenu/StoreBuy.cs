using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBuy : MonoBehaviour {


	public int money;
	public int cucuSeedAmount;
	public bool asd = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		money = VegetableMainMessange.money;
		cucuSeedAmount = cucumber.seedAmount;
	}

	public void Buy(){
		asd = true;
		if(gameObject.tag=="cucumber"){
			if(VegetableMainMessange.money>=cucumber.buyingPrice){
				VegetableMainMessange.money -= cucumber.buyingPrice;
				cucumber.seedAmount += 5;
			}
		}
		if(gameObject.tag=="eggplant"){
			if(VegetableMainMessange.money>=eggplant.buyingPrice){
				VegetableMainMessange.money -= eggplant.buyingPrice;
				eggplant.seedAmount += 5;
			}
		}
		if(gameObject.tag=="carrot"){
			if(VegetableMainMessange.money>=carrot.buyingPrice){
				VegetableMainMessange.money -= carrot.buyingPrice;
				carrot.seedAmount += 5;
			}
		}
		if(gameObject.tag=="pumpkin"){
			if(VegetableMainMessange.money>=pumpkin.buyingPrice){
				VegetableMainMessange.money -= pumpkin.buyingPrice;
				pumpkin.seedAmount += 5;
			}
		}
		if(gameObject.tag=="tomato"){
			if(VegetableMainMessange.money>=tomato.buyingPrice){
				VegetableMainMessange.money -= tomato.buyingPrice;
				tomato.seedAmount += 5;
			}
		}
	}

}
