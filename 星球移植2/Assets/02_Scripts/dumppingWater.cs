using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dumppingWater : MonoBehaviour {

	//public GameObject waterParticle;
	public int dumpping;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//判斷合理傾斜角度才讓dumpping+1
		if (transform.eulerAngles.z > 30 && transform.eulerAngles.z < 200) {
			dumpping += 1;
		}
		//拿正時dumpping設為0
		if (transform.eulerAngles.z < 30 && transform.eulerAngles.z > -160) {
			dumpping = 0;
		}
		//當傾協時播放waterParticle
		if(dumpping==1){
			gameObject.GetComponentInChildren<ParticleSystem> ().Play ();
		}
		//當dumpping=0時，停止播放waterParticle
		if (dumpping==0) {
			gameObject.GetComponentInChildren<ParticleSystem>().Stop();
		}
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