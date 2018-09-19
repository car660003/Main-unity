using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterLevel : MonoBehaviour {

	public float water = 0;
	public bool a =false;

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
		if(waterParticle.tag.Equals("WaterParticle")){
			water += Time.deltaTime;
			a = true;
		}
	}
}
