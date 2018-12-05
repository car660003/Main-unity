using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantCanvasSeedCount : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.tag=="pumpkin"){
			if(pumpkin.seedAmount<0){
				text.text = "0";
			}else if(pumpkin.seedAmount>=0){
				text.text = pumpkin.seedAmount.ToString();
			}

		}else if(gameObject.tag=="cucumber"){
			if(cucumber.seedAmount<0){
				text.text = "0";
			}else if(cucumber.seedAmount>=0){
				text.text = cucumber.seedAmount.ToString();
			}

		}else if(gameObject.tag=="carrot"){
			if(carrot.seedAmount<0){
				text.text = "0";
			}else if(carrot.seedAmount>=0){
				text.text = carrot.seedAmount.ToString();
			}

		}else if(gameObject.tag=="eggplant"){
			if(eggplant.seedAmount<0){
				text.text = "0";
			}else if(eggplant.seedAmount>=0){
				text.text = eggplant.seedAmount.ToString();
			}

		}else if(gameObject.tag=="tomato"){
			if(tomato.seedAmount<0){
				text.text = "0";
			}else if(tomato.seedAmount>=0){
				text.text = tomato.seedAmount.ToString();
			}
		}
	}
}
