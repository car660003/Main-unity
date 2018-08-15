using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createCrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider seed){ //aaa為自定義碰撞事件

		if (seed.gameObject.name == "pumpkinSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed

			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)

			GameObject pfb = Resources.Load ("pumpkin_01") as GameObject;
			GameObject prefabInstance = Instantiate (pfb);
			prefabInstance.transform.parent = this.transform;//設為子物件
			prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
			prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
			//StartCoroutine (toPlantState02 (10));
		}
	}
}
