using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageVeg : MonoBehaviour {

	public int pumpkinCount;
	public int cucumberCount;
	public int carrotCount;
	public int eggplantCount;
	public int tomatoCount;

	public bool changeFlag = false;

	public GameObject PathA;//起點
	public GameObject PathB;//終點
	public GameObject Obj;//要移動的物件
	public float speed = 1.0f;//移動速度
	private float firstSpeed;//紀錄第一次移動的距離
	public float tmp;

	public Vector3 firstPathObj;//出現位置
	public Vector3 secPathObj;//丟到出現位置的上面

	public bool bbbb;

	// Use this for initialization
	void Start () {
		// PathA 和 PathB 的距離乘上 speed
		firstSpeed = Vector3.Distance(Obj.transform.position, PathB.transform.position) * speed;
		firstPathObj = gameObject.transform.position;//把當前位置丟到出現位置
		secPathObj = new Vector3 (firstPathObj.x, firstPathObj.y + 200.0f, firstPathObj.z);//計算出現位置的上面
		gameObject.transform.position = secPathObj;//把物件丟到當前位置的上面
	}
	
	// Update is called once per frame
	void Update () {
		tmp = Vector3.Distance(Obj.transform.position, PathB.transform.position);//計算物件到終點的距離


		if((pumpkin.Count-pumpkinCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-南瓜");
			//把物件丟到出現位置後，停頓一秒
			gameObject.transform.position = firstPathObj;
			StartCoroutine ("delaySec");
		}else if((cucumber.Count-cucumberCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-番茄");
			//把物件丟到出現位置後，停頓一秒
			gameObject.transform.position = firstPathObj;
			StartCoroutine ("delaySec");
		}else if((carrot.Count-carrotCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-紅蘿蔔");
			//把物件丟到出現位置後，停頓一秒
			gameObject.transform.position = firstPathObj;
			StartCoroutine ("delaySec");
		}else if((eggplant.Count-eggplantCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-茄子");
			//把物件丟到出現位置後，停頓一秒
			gameObject.transform.position = firstPathObj;
			StartCoroutine ("delaySec");
		}else if((tomato.Count-tomatoCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-番茄");
			//把物件丟到出現位置後，停頓一秒
			gameObject.transform.position = firstPathObj;
			StartCoroutine ("delaySec");
		}

		//讓UIImageVegetable移動
		if(changeFlag){
			Obj.transform.position = Vector3.Lerp(Obj.transform.position, PathB.transform.position, speed*Time.deltaTime);
			bbbb = true;
			//speed = calculateNewSpeed();
		}
		//判斷距離很近時，把物件丟到出現位置的上面
		if (tmp <= 10) {
			changeFlag = false;
			gameObject.transform.position = secPathObj;
		}

		pumpkinCount = pumpkin.Count;
		cucumberCount = cucumber.Count;
		carrotCount = carrot.Count;
		eggplantCount = eggplant.Count;
		tomatoCount = tomato.Count;

	}
	IEnumerator delaySec(){
		yield return new WaitForSeconds (1);
		changeFlag = true;
	}

	/*private float calculateNewSpeed(){
		//因為每次移動都是 Obj 在移動，所以要取得 Obj 和 PathB 的距離
		tmp = Vector3.Distance(Obj.transform.position, PathB.transform.position);

		//當距離為0的時候，就代表已經移動到目的地了。
		if (tmp == 0) {
			changeFlag = false;
			return tmp;
		}else
			return (firstSpeed / tmp);
	}*/
}
