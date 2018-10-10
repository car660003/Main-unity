using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class identifySeed : MonoBehaviour {
	
	public waterLevel waterLevel;
	public bool isPlanting = false;
	// Use this for initialization
	void Start () {
		waterLevel = gameObject.GetComponent<waterLevel>();
	}
	
	// Update is called once per frame
	void Update () {
		isPlanting = waterLevel.isPlanting;//用waterLevel的isPlanting判斷當下是否處於種植中
	}

	void OnTriggerEnter(Collider seed){ //aaa為自定義碰撞事件

		if(isPlanting==false){
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
			}else if (seed.gameObject.tag == "tomatoSeed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
				gameObject.AddComponent<createEggplant> ();
				Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)
			}
		}

	}
}
