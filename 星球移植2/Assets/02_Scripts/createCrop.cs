using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class createCrop : MonoBehaviour {

	public string plantingTime_toString;
	public string plantingTime_toString2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider seed){ //aaa為自定義碰撞事件

		if (seed.gameObject.name == "pumpkinSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed

			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)

			DateTime plantingTime = DateTime.Now;//把種植當下時間存起來
			DateTime aaa = plantingTime.AddMinutes(5);

			plantingTime_toString = aaa.ToString ();//把種植當下時間列出來
			plantingTime_toString2 = aaa.ToString ();

			GameObject pfb = Resources.Load ("pumpkin/pumpkin_01") as GameObject;//產生Pumpkin
			GameObject prefabInstance = Instantiate (pfb);
			prefabInstance.transform.parent = this.transform;//設為子物件
			prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
			//prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
			//StartCoroutine (toPlantState02 (10));
		}
	}


}
