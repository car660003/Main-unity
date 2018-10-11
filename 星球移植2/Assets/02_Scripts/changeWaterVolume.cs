using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeWaterVolume : MonoBehaviour {

	public float distance;
	public waterLevel waterLevel;
	public bool isPlanting;

	public Material meshRender = null;
	public Renderer rend;

	public createPumpkin createPumpkin;
	public createCarrot createCarrot;
	public createTomato createTomato;
	public createCucumber createCucumber;
	public createEggplant createEggplant;
	public float hp;


	//public Texture texture;

	// Use this for initialization
	void Start () {
		waterLevel = gameObject.GetComponentInParent<waterLevel>();
		meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
		rend =  GetComponentInChildren<Renderer>();
		rend.material = meshRender;
	}
	
	// Update is called once per frame
	void Update () {
		createPumpkin = gameObject.GetComponentInParent<createPumpkin>();
		createCarrot = gameObject.GetComponentInParent<createCarrot>();
		createTomato = gameObject.GetComponentInParent<createTomato>();
		createCucumber = gameObject.GetComponentInParent<createCucumber>();
		createEggplant = gameObject.GetComponentInParent<createEggplant>();

		if(createPumpkin){
			hp = createPumpkin.hp;
			if(hp>=pumpkin.hp){
				meshRender = Resources.Load("showWaterVolume/Materials/100水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(pumpkin.hp*0.75)&&hp<=(pumpkin.hp*0.99)){
				meshRender = Resources.Load("showWaterVolume/Materials/75水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(pumpkin.hp*0.50)&&hp<=(pumpkin.hp*0.74)){
				meshRender = Resources.Load("showWaterVolume/Materials/50水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(pumpkin.hp*0.25)&&hp<=(pumpkin.hp*0.49)){
				meshRender = Resources.Load("showWaterVolume/Materials/25水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(pumpkin.hp*0.10)&&hp<=(pumpkin.hp*0.24)){
				meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
				rend.material = meshRender;
			}
		}else if(createCarrot){
			hp = createCarrot.hp;
			if(hp>=carrot.hp){
				meshRender = Resources.Load("showWaterVolume/Materials/100水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(carrot.hp*0.75)&&hp<=(carrot.hp*0.99)){
				meshRender = Resources.Load("showWaterVolume/Materials/75水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(carrot.hp*0.50)&&hp<=(carrot.hp*0.74)){
				meshRender = Resources.Load("showWaterVolume/Materials/50水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(carrot.hp*0.25)&&hp<=(carrot.hp*0.49)){
				meshRender = Resources.Load("showWaterVolume/Materials/25水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(carrot.hp*0.10)&&hp<=(carrot.hp*0.24)){
				meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
				rend.material = meshRender;
			}
		}else if(createTomato){
			hp = createTomato.hp;
			if(hp>=tomato.hp){
				meshRender = Resources.Load("showWaterVolume/Materials/100水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(tomato.hp*0.75)&&hp<=(tomato.hp*0.99)){
				meshRender = Resources.Load("showWaterVolume/Materials/75水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(tomato.hp*0.50)&&hp<=(tomato.hp*0.74)){
				meshRender = Resources.Load("showWaterVolume/Materials/50水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(tomato.hp*0.25)&&hp<=(tomato.hp*0.49)){
				meshRender = Resources.Load("showWaterVolume/Materials/25水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(tomato.hp*0.10)&&hp<=(tomato.hp*0.24)){
				meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
				rend.material = meshRender;
			}
		}else if(createCucumber){
			hp = createCucumber.hp;
			if(hp>=cucumber.hp){
				meshRender = Resources.Load("showWaterVolume/Materials/100水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(cucumber.hp*0.75)&&hp<=(cucumber.hp*0.99)){
				meshRender = Resources.Load("showWaterVolume/Materials/75水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(cucumber.hp*0.50)&&hp<=(cucumber.hp*0.74)){
				meshRender = Resources.Load("showWaterVolume/Materials/50水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(cucumber.hp*0.25)&&hp<=(cucumber.hp*0.49)){
				meshRender = Resources.Load("showWaterVolume/Materials/25水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(cucumber.hp*0.10)&&hp<=(cucumber.hp*0.24)){
				meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
				rend.material = meshRender;
			}
		}else if(createEggplant){
			hp = createEggplant.hp;
			if(hp>=eggplant.hp){
				meshRender = Resources.Load("showWaterVolume/Materials/100水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(eggplant.hp*0.75)&&hp<=(eggplant.hp*0.99)){
				meshRender = Resources.Load("showWaterVolume/Materials/75水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(eggplant.hp*0.50)&&hp<=(eggplant.hp*0.74)){
				meshRender = Resources.Load("showWaterVolume/Materials/50水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(eggplant.hp*0.25)&&hp<=(eggplant.hp*0.49)){
				meshRender = Resources.Load("showWaterVolume/Materials/25水量") as Material;
				rend.material = meshRender;
			}else if(hp>=(eggplant.hp*0.10)&&hp<=(eggplant.hp*0.24)){
				meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
				rend.material = meshRender;
			}
		}

		isPlanting = waterLevel.isPlanting;


		//distance = Vector3.Distance (Camera.main.transform.position, transform.position);
		//if (distance <= 10/*&&waterLevel.isPlanting==true/*&&waterLevel.water>=3*/) {
			gameObject.SetActive (true);
		//} else {
		//	gameObject.SetActive (false);
		//}

	}
}
