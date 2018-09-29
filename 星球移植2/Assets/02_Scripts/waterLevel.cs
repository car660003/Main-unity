using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterLevel : MonoBehaviour {

	public float water = 0;
	public bool a =false;
	public bool isPlanting = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnParticleCollision(GameObject waterParticle){
		/*if(gameObject.GetComponents<createPumpkin>()){
			water += Time.deltaTime;
		}*/
		//鎖定種植後加水才會增加水量
		if(waterParticle.tag.Equals("WaterParticle")){
			if(isPlanting){
				water += Time.deltaTime;
				a = true;
			}
		}
	}
}
