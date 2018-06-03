using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiveParent_method2 : MonoBehaviour {

	//public GameObject prefab;
	private bool isPlanting = false;
	public GameObject player;
	public Transform player_transform;
	public Text grass_Text;
	public Text harvest_Text;
	public GameObject thisName;

	//時間相關
	public int plantingTime;
	public bool timerGo = false;

	// Use this for initialization
	void Start () {
		string thisName = gameObject.name.ToString ();
		//grass_Text = GameObject.Find ("Cube01/Canvas/grass_Text");
		//grass_Text = gameObject.transform.GetChild(0).gameObject;
	}

	// Update is called once per frame
	void Update () {
		//GameObject playerChoice_here = VegetableMainMessange.playerChoice;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		float distance = Vector3.Distance( player_transform.position, transform.position );

		//判斷player和cube距離為2以內、滑鼠有點擊到cube，才作生長的動作
		if (distance <= 2 && Input.GetMouseButton(0) && Physics.Raycast(ray,out hit) && hit.collider.tag.Contains("field")) {
			if (!isPlanting) {//用來限制只能執行一次
				//GameObject container = GameObject.Find (thisName);
				GameObject pfb = Resources.Load (example01.growingState01) as GameObject;
				GameObject prefabInstance = Instantiate (pfb);
				prefabInstance.transform.parent = thisName.transform;//設為子物件
				prefabInstance.transform.position = new Vector3 (thisName.transform.position.x, 0.4f, 0.0f);
				StartCoroutine (wait (example01.growingTime+2));
				plantingTime = example01.growingTime;
				timerGo = true;
			}
			isPlanting = true;
		}
		if(timerGo){//顯示剩餘時間
			StartCoroutine (showTime (1));
		}
	}

	IEnumerator wait(float s){
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		//GameObject container = GameObject.Find("Cube01");
		GameObject pfb = Resources.Load("grass_02") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = thisName.transform;
		prefabInstance.transform.position = new Vector3(thisName.transform.position.x,0.4f,0.0f);
		harvest_Text.text = "你的收成：1";
	}

	IEnumerator showTime(float s){
		timerGo = false;
		yield return new WaitForSeconds (s);
		grass_Text.text = plantingTime.ToString ();
		if(plantingTime<=0){
			grass_Text.text = "成熟!";
		}
		plantingTime--;
		timerGo = true;
	}
}