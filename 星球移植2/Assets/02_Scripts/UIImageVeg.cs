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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



		if((pumpkin.Count-pumpkinCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-南瓜");
		}else if((cucumber.Count-cucumberCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-番茄");
		}else if((carrot.Count-carrotCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-紅蘿蔔");
		}else if((eggplant.Count-eggplantCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-茄子");
		}else if((tomato.Count-tomatoCount)>=1){
			gameObject.GetComponent<SpriteRenderer> ().sprite = (Sprite)Resources.Load<Sprite> ("UI/收成圖案-番茄");
		}

		pumpkinCount = pumpkin.Count;
		cucumberCount = cucumber.Count;
		carrotCount = carrot.Count;
		eggplantCount = eggplant.Count;
		tomatoCount = tomato.Count;

	}
}
