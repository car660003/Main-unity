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
			text.text = pumpkin.seedAmount.ToString();
		}else if(gameObject.tag=="cucumber"){
			text.text = cucumber.seedAmount.ToString();
		}else if(gameObject.tag=="carrot"){
			text.text = carrot.seedAmount.ToString();
		}else if(gameObject.tag=="eggplant"){
			text.text = eggplant.seedAmount.ToString();
		}else if(gameObject.tag=="tomato"){
			text.text = tomato.seedAmount.ToString();
		}
	}
}
