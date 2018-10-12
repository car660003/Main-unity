using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changFields_materials : MonoBehaviour {

	public waterLevel waterLevel;
	public Material meshRender = null;
	public Renderer rend;
	/*public float field01;
	public float field01Alpha = 255.0f;

	public float field02;
	public float field02Alpha = 0.0f;*/

	// Use this for initialization
	void Start () {
		waterLevel = gameObject.GetComponentInParent<waterLevel>();
		meshRender = Resources.Load("fields/土_2") as Material;
		rend =  GetComponentInChildren<Renderer>();
		rend.material = meshRender;
		//field01 = gameObject.GetComponentInChildren<Renderer> ().materials[0].color.a;
		//field02 = gameObject.GetComponentInChildren<Renderer> ().materials[1].color.a;
	}

	// Update is called once per frame
	void Update () {
		if(waterLevel.water>=3){
			meshRender = Resources.Load("fields/土") as Material;
			rend.material = meshRender;
		}else if(waterLevel.water<=3){
			meshRender = Resources.Load("fields/土_2") as Material;
			rend.material = meshRender;
		}

		/*if(waterLevel.water>=3){
			field01Alpha = field01Alpha - 10.0f;
			field01 = field01Alpha / 255.0f;
			gameObject.GetComponentInChildren<Renderer> ().materials[0].color.a = field01;

			field02Alpha = field02Alpha + 10.0f;
			field02 = field02Alpha / 255.0f;
			gameObject.GetComponentInChildren<Renderer> ().materials [1].color.a = field02;
		}else if(waterLevel.water<=3){
			
		}*/


	}
}
