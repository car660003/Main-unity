using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class identifySeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider seed){ //aaa為自定義碰撞事件


		if (seed.gameObject.tag == "pumpkinSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
			gameObject.AddComponent<createPumpkin> ();
			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)
		}else if (seed.gameObject.tag == "cucumberSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
			gameObject.AddComponent<createCucumber> ();
			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)
		}else if (seed.gameObject.tag == "carrotSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
			gameObject.AddComponent<createCarrot> ();
			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)
		}else if (seed.gameObject.tag == "eggplantSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
			gameObject.AddComponent<createEggplant> ();
			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)
		}
	}
}
