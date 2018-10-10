using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class createCarrot : MonoBehaviour {

	public string plantingTime_toString;//種植當下的時間
	DateTime plantingTime;//種植時間

	public string matureTime_toString;//植物成熟的時間(計算的值)
	DateTime matureTime;//植物成熟的時間(計算的值)

	public string remainingSeconds_toString = "";
	public bool isPlanting = false;
	public bool changeStatus = false;
	[SerializeField]
	GrowingSTATUS growingSTATUS;
	public waterLevel waterLevel;
	public float water;

	public float hp;



	enum GrowingSTATUS{
		empty,
		Growing_01,
		Growing_02,
		Growing_03,
		Growing_04,
		Growing_Die
	}
	// Use this for initialization
	void Start () {
		growingSTATUS = GrowingSTATUS.empty;
		waterLevel = gameObject.GetComponent<waterLevel>();
		waterLevel.isPlanting = true;	//鎖定種植後加水才會增加水量
		hp = carrot.hp;	//獲得VegetableDetail_Static裡面pumpkin的生命值
		countMatureTime ();
	}
	void Awake(){
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlanting){//種植之後才會計算距離當階段成熟前的剩餘秒數
			remainingSeconds_toString = matureTime.Subtract (DateTime.Now).Duration ().ToString ();
			plantingTime_toString = plantingTime.ToString ();//把種植當下時間列出來
			matureTime_toString = matureTime.ToString ();

			hp += waterLevel.water - water;	//每秒增加的水量加上目前的hp值
			hp -= Time.deltaTime / carrot.hpPerS;	//每秒扣一部份的hp
		}

		if(hp<=0){//判定血量<=0，執行死亡
			changeStatus = true;
			growingSTATUS = GrowingSTATUS.Growing_Die;
			isPlanting = false;
			remainingSeconds_toString = "已死亡!";
		}

		water = waterLevel.water;
		//判斷當前狀態，並執行要做的事情
		switch (growingSTATUS) {
			case GrowingSTATUS.empty:
				if(water>=3){
					isPlanting = true;
					if(DateTime.Compare(DateTime.Now,matureTime)>0){//若現在時間超過當階段成熟時間,改變stutas至下一個階段
						growingSTATUS = GrowingSTATUS.Growing_01;
						countMatureTime ();
						changeStatus = true;//允許改變status
					}
				}
				break;

			case GrowingSTATUS.Growing_01:
				if (changeStatus) {	//如果允許改變stutas，執行PumpkinGrowing_01 ();，並不允許改變stutas
					CarrotGrowing_01 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//若現在時間超過當階段成熟時間,改變stutas至下一個階段
					growingSTATUS = GrowingSTATUS.Growing_02;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case GrowingSTATUS.Growing_02:
				if (changeStatus) {	
					CarrotGrowing_02 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					growingSTATUS = GrowingSTATUS.Growing_03;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case GrowingSTATUS.Growing_03:
				if (changeStatus) {	
					CarrotGrowing_03 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					growingSTATUS = GrowingSTATUS.Growing_04;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case GrowingSTATUS.Growing_04:
				if (changeStatus) {	
					CarrotGrowing_04 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					remainingSeconds_toString = "已成熟!";
				}
					break;

			case GrowingSTATUS.Growing_Die:
				if (changeStatus) {	
					CarrotGrowing_die ();
					changeStatus = false;
				}
				break;
		}
	}

	public void countMatureTime(){//計算當下階段剩餘時間
		plantingTime = DateTime.Now;//把種植當下時間存起來

		switch (growingSTATUS) {
		case GrowingSTATUS.empty:
			matureTime = plantingTime.AddSeconds(carrot.growing_00To01_time);//植物當下階段應當成熟成熟的時間(計算的值)
			break;
		case GrowingSTATUS.Growing_01:
			matureTime = plantingTime.AddSeconds(carrot.growing_01To02_time);//植物當下階段應當成熟成熟的時間(計算的值)
			break;
		case GrowingSTATUS.Growing_02:
			matureTime = plantingTime.AddSeconds(carrot.growing_02To03_time);
			break;
		case GrowingSTATUS.Growing_03:
			matureTime = plantingTime.AddSeconds(carrot.growing_03To04_time);
			break;
		}
	}

	//生成物件
	public void CarrotGrowing_01(){
		GameObject pfb = Resources.Load ("carrot/carrot_01") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);

	}
	public void CarrotGrowing_02(){
		Destroy (transform.GetChild (2).gameObject);
		GameObject pfb = Resources.Load ("carrot/carrot_02") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
	}
	public void CarrotGrowing_03(){
		Destroy (transform.GetChild (2).gameObject);
		GameObject pfb = Resources.Load ("carrot/carrot_03") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
	}
	public void CarrotGrowing_04(){
		Destroy (transform.GetChild (2).gameObject);
		GameObject pfb = Resources.Load ("carrot/carrot_04") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
	}
	public void CarrotGrowing_die(){
		Destroy (transform.GetChild (2).gameObject);
		GameObject pfb = Resources.Load ("carrot/carrot_die") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
	}
}