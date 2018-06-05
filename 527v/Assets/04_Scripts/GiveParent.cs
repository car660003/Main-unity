using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GiveParent : MonoBehaviour {

	//public GameObject prefab;
	public bool isPlanting = false;
	public GameObject player;
	public Transform player_transform;
	public Text grass_Text;
	public Text harvest_Text;

	//時間相關
	public int plantingTime;
	public bool timerGo = false;

	// Use this for initialization
	void Start () {
		//第一种
		//GameObject prefabInstance = Instantiate(prefab);
		//prefabInstance.transform.parent = container.transform;
		//第二种
		//players = GameObject.FindGameObjectswithTag("player");
		plantingTime = 5;
	}

	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		float distance = Vector3.Distance( player_transform.position, transform.position );

		//判斷player和cube距離為2以內、滑鼠有點擊到cube，才作生長的動作
		if (distance <= 2 && Input.GetMouseButton(0) && Physics.Raycast(ray,out hit) && hit.collider.name.Contains("Cube01")) {
			if (!isPlanting) {//用來限制只能執行一次
				GameObject container = GameObject.Find ("Cube01");
				GameObject pfb = Resources.Load ("grass_01") as GameObject;
				GameObject prefabInstance = Instantiate (pfb);
				prefabInstance.transform.parent = container.transform;//設為子物件
				prefabInstance.transform.position = new Vector3 (container.transform.position.x, 0.4f, 0.0f);
				StartCoroutine (wait (5));
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
		GameObject container = GameObject.Find("Cube01");
		GameObject pfb = Resources.Load("grass_02") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = container.transform;
		prefabInstance.transform.position = new Vector3(container.transform.position.x,0.4f,0.0f);
		harvest_Text.text = "你的收成：1";
	}

	IEnumerator showTime(float s){
		timerGo = false;
		yield return new WaitForSeconds (s);
		plantingTime--;
		grass_Text.text = plantingTime.ToString ();
		if(plantingTime<=0){
			grass_Text.text = "成熟!";
		}
		timerGo = true;
	}
}