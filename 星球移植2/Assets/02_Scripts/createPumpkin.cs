using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class createPumpkin : MonoBehaviour {

	public string remainingSeconds_toString = "";

	DateTime plantingTime;//種植時間
	public string plantingTime_toString;//種植當下的時間

	DateTime matureTime;//植物成熟的時間(計算的值)
	public string matureTime_toString;//植物成熟的時間(計算的值)

	public bool isPlanting = false;
	public bool changeStatus = false;
	[SerializeField]
	PumpkinSTATUS pumpkinStatus;
	public waterLevel waterLevel;
	public float water;

	public float hp = 100;


	enum PumpkinSTATUS{
		empty,
		PumpkinGrowing_01,
		PumpkinGrowing_02,
		PumpkinGrowing_03,
		PumpkinGrowing_04,
		PumpkinGrowing_Die
	}

	// Use this for initialization
	void Start () {
		pumpkinStatus = PumpkinSTATUS.empty;
		waterLevel = gameObject.GetComponent<waterLevel>();

	}
	void Awake(){
		
	}
	
	// Update is called once per frame
	void Update () {
		//種植之後才會計算距離當階段成熟前的剩餘秒數
		if(isPlanting){
			remainingSeconds_toString = matureTime.Subtract (DateTime.Now).Duration ().ToString ();
			plantingTime_toString = plantingTime.ToString ();//把種植當下時間列出來
			matureTime_toString = matureTime.ToString ();

			hp += (waterLevel.water-water)*2;
			hp -= Time.deltaTime/2;
		}

		water = waterLevel.water; //獲得waterLevel裡面water變數

		//判斷當前狀態，並執行要做的事情
		switch (pumpkinStatus) { 
			case PumpkinSTATUS.empty:
				if(water>=3){
					pumpkinStatus = PumpkinSTATUS.PumpkinGrowing_01;
					countMatureTime ();
					isPlanting = true;//判斷是否有種植作物
					changeStatus = true;//允許改變status
				}
				break;

			case PumpkinSTATUS.PumpkinGrowing_01:
				//countMatureTime ();
				if (changeStatus) {	//如果允許改變stutas，執行PumpkinGrowing_01 ();，並不允許改變stutas
					PumpkinGrowing_01 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//若現在時間超過當階段成熟時間,改變stutas至下一個階段
					pumpkinStatus = PumpkinSTATUS.PumpkinGrowing_02;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case PumpkinSTATUS.PumpkinGrowing_02:
				if (changeStatus) {	
					PumpkinGrowing_02 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					pumpkinStatus = PumpkinSTATUS.PumpkinGrowing_03;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case PumpkinSTATUS.PumpkinGrowing_03:
				if (changeStatus) {	
					PumpkinGrowing_03 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					pumpkinStatus = PumpkinSTATUS.PumpkinGrowing_04;
					countMatureTime ();
					changeStatus = true;
				}
					break;

			case PumpkinSTATUS.PumpkinGrowing_04:
				if (changeStatus) {	
					PumpkinGrowing_04 ();
					changeStatus = false;
				}
				if(DateTime.Compare(DateTime.Now,matureTime)>0){//現在時間超過成熟時間
					remainingSeconds_toString = "已成熟!";
				}
					break;
		}
	}

	//計算當下階段剩餘時間
	public void countMatureTime(){//計算當下階段剩餘時間
		plantingTime = DateTime.Now;//把種植當下時間存起來

		switch (pumpkinStatus) {
		case PumpkinSTATUS.PumpkinGrowing_01:
			matureTime = plantingTime.AddSeconds(180);//植物當下階段應當成熟成熟的時間(計算的值)
			break;
		case PumpkinSTATUS.PumpkinGrowing_02:
			matureTime = plantingTime.AddSeconds(180);
			break;
		case PumpkinSTATUS.PumpkinGrowing_03:
			matureTime = plantingTime.AddSeconds(180);
			break;
		}
	}

	//生成物件
	public void PumpkinGrowing_01(){
		GameObject pfb = Resources.Load ("pumpkin/pumpkin_01") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
		//float rot = Random.rotation.z;
		/*Quaternion quate = Quaternion.identity;
		float Rot_z = Random.rotation.z;
		quate.eulerAngles = new Vector3 (0, 0, Rot_z);
		prefabInstance.transform.rotation = quate;*/
		/*var rot = Random.rotation.z;
		prefabInstance.transform.rotation = Quaternion.Euler (0f,0f,rot);*/
	}
	public void PumpkinGrowing_02(){
		Destroy (transform.GetChild (1).gameObject);
		GameObject pfb = Resources.Load ("pumpkin/pumpkin_02") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
	}
	public void PumpkinGrowing_03(){
		Destroy (transform.GetChild (1).gameObject);
		GameObject pfb = Resources.Load ("pumpkin/pumpkin_03") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
	}
	public void PumpkinGrowing_04(){
		Destroy (transform.GetChild (1).gameObject);
		GameObject pfb = Resources.Load ("pumpkin/pumpkin_04") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		prefabInstance.transform.parent = this.transform;//設為子物件
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
	}
}

	/*IEnumerator toPlantState02(float s){//幾秒後，跳轉到下一個程式
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		GameObject pfb = Resources.Load("pumpkin/pumpkin_02") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
		//prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
		StartCoroutine (toPlantState03 (5));
	}
	IEnumerator toPlantState03(float s){
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		GameObject pfb = Resources.Load("pumpkin/pumpkin_03") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
		//prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
		StartCoroutine (toPlantState04 (9));
	}
	IEnumerator toPlantState04(float s){
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		GameObject pfb = Resources.Load("pumpkin/pumpkin_04") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = this.transform;
		prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y+0.1f, this.transform.position.z);
		//prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
	}*/

