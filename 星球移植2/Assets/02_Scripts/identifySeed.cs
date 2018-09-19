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
		}
	}
}
