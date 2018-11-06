using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseCanvasVegtableCount : MonoBehaviour {

	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.tag=="pumpkin"){
			text.text = pumpkin.Count.ToString();
		}else if(gameObject.tag=="cucumber"){
			text.text = cucumber.Count.ToString();
		}else if(gameObject.tag=="carrot"){
			text.text = carrot.Count.ToString();
		}else if(gameObject.tag=="eggplant"){
			text.text = eggplant.Count.ToString();
		}else if(gameObject.tag=="tomato"){
			text.text = tomato.Count.ToString();
		}
	}
}
