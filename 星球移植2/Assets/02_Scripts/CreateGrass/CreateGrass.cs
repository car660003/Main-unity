using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrass : MonoBehaviour {
	
	public int maxGrass = 8;//雜草的最大數量
	public int nowGrass;//目前雜草的個數

	public bool[] checkGrass;//存放某個點是否已長雜草

	public GameObject[] Objects; //裝生成物件的陣列。

	public Transform[] Points; //裝生成點的陣列。

	public float Ins_Time = 1; //每幾秒生成一次。

	// Use this for initialization
	void Start () {
		//重複呼叫(“函式名”，第一次間隔幾秒呼叫，每幾秒呼叫一次)。
		InvokeRepeating("Ins_Objs", Ins_Time, Ins_Time);
		for(int i=0;i<Points.Length-1;i++){
			checkGrass [i] = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//生成物件函式。
	void Ins_Objs(){

		//隨機產生0~物件陣列長度的整數-1(不包含最大值所以-1)。
		int Random_Objects = Random.Range(0, Objects.Length);

		//隨機產生0~生成點陣列長度的整數-1(不包含最大值所以-1)。
		int Random_Points = Random.Range(0, Points.Length);

		if(checkGrass[Random_Points]==false){

			if(nowGrass<maxGrass){
				//Instantiate實例化(要生成的物件, 物件位置, 物件旋轉值);
				GameObject prefabInstance = Instantiate(Objects[Random_Objects], Points[Random_Points].transform.position, Points [Random_Points].transform.rotation);
				prefabInstance.transform.parent = Points[Random_Points].transform;//設為子物件
				nowGrass++;
				checkGrass [Random_Points] = true;
			}
		}


	}


}
