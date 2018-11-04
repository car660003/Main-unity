using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumppingWater : MonoBehaviour {

	//public GameObject waterParticle;
	public int dumpping;
	public float maxWaterVolume = 50;
	public float waterVolume = 30;

	public GameObject insideWater;
	public float insideWaterHigh;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//判斷合理傾斜角度才讓dumpping+1
		if (transform.eulerAngles.z > 30 && transform.eulerAngles.z < 200) {
			dumpping += 1;
			waterVolume = waterVolume - Time.deltaTime;
		}
		//拿正時dumpping設為0
		if (transform.eulerAngles.z < 30 && transform.eulerAngles.z > -160) {
			dumpping = 0;
		}
		//當傾協時播放waterParticle
		if(dumpping==1){
			gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
		}else if (dumpping==0) {
			//當dumpping=0時，停止播放waterParticle
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		}

		//水面無法設定到正確的位置

		/*//當傾協時播放waterParticle
		if(dumpping==1&&waterVolume>=0){
			gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
		}else if (dumpping==0||waterVolume<0) {
			//當dumpping=0時，停止播放waterParticle
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		}*/

		//判斷水量大小，調整水面高低
		/*if(waterVolume>=maxWaterVolume*0.75){
			//insideWaterHigh = 0.278f;
			//insideWater.transform.position.y = insideWaterHigh;
			//insideWater.transform.position = new Vector3(insideWater.transform.position.x,this.transform.position.y+0.278f, this.transform.position.z);
			Vector3 temp = insideWater.transform.position; // copy to an auxiliary variable...
			temp.y = 0.278f; // modify the component you want in the variable...
			insideWater.transform.position = temp; // and save the modified value 
			
		}else if(waterVolume>=maxWaterVolume*0.4&&waterVolume<=maxWaterVolume*0.74){
			//insideWaterHigh = 0.007f;
			//insideWater.transform.position.y = insideWaterHigh;
			//insideWater.transform.position = new Vector3(insideWater.transform.position.x,this.transform.position.y+0.007f, this.transform.position.z);
			Vector3 temp = insideWater.transform.position; // copy to an auxiliary variable...
			temp.y = 0.007f; // modify the component you want in the variable...
			insideWater.transform.position = temp; // and save the modified value 

		}else if(waterVolume>=maxWaterVolume*0.001&&waterVolume<=maxWaterVolume*0.39){
			//insideWaterHigh = -0.47f;
			//insideWater.transform.position.y = insideWaterHigh;
			//insideWater.transform.position = new Vector3(insideWater.transform.position.x,this.transform.position.y-0.47f, this.transform.position.z);
			Vector3 temp = insideWater.transform.position; // copy to an auxiliary variable...
			temp.y = -0.47f; // modify the component you want in the variable...
			insideWater.transform.position = temp; // and save the modified value 

		}else if(waterVolume<=0){
			//insideWaterHigh = -0.655f;
			//insideWater.transform.position.y = insideWaterHigh;
			//insideWater.transform.position = new Vector3(insideWater.transform.position.x,this.transform.position.y-0.655f, this.transform.position.z);
			Vector3 temp = insideWater.transform.position; // copy to an auxiliary variable...
			temp.y = -0.655f; // modify the component you want in the variable...
			insideWater.transform.position = temp; // and save the modified value 

		}*/
	}
}


//紀錄
//當傾斜時生成waterParticle，並設定位置  bug：當向後仰時(角度-160度)，生成的waterParticle是錯的!!!!!
/*GameObject waterParticleInstance = Instantiate (waterParticle);
			waterParticleInstance.transform.parent = this.transform;
			//設定位置
			waterParticleInstance.transform.position = new Vector3 (this.transform.position.x-1.5f,this.transform.position.y+0.1f,this.transform.position.z);
			//設定角度
			Quaternion quate = Quaternion.identity;
			float Rot_y = gameObject.transform.localRotation.eulerAngles.y;
			quate.eulerAngles = new Vector3 (0, Rot_y-90, 0);
			waterParticleInstance.transform.rotation = quate;*/