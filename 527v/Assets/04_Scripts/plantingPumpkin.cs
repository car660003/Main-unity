using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class plantingPumpkin : MonoBehaviour {

	// Use this for initialization
	public GameObject thisField;



	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider seed){ //aaa為自定義碰撞事件
	    
		if (seed.gameObject.tag == "Seed"){ //如果aaa碰撞事件的物件標籤名稱是Seed
		    
			Destroy(seed.gameObject); //刪除碰撞到的物件(Seed)

			GameObject pfb = Resources.Load ("pumpkin_01") as GameObject;
			GameObject prefabInstance = Instantiate (pfb);
			prefabInstance.transform.parent = thisField.transform;//設為子物件
			prefabInstance.transform.position = new Vector3 (thisField.transform.position.x, thisField.transform.position.y, thisField.transform.position.z);
			prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
			StartCoroutine (toPlantState02 (10));
			//Debug.Log ("執行01");
		}
	}
	IEnumerator toPlantState02(float s){
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		GameObject pfb = Resources.Load("pumpkin_02") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = thisField.transform;
		prefabInstance.transform.position = new Vector3 (thisField.transform.position.x, thisField.transform.position.y, thisField.transform.position.z);
		prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);
		StartCoroutine (toPlantState03 (10));
	}
	IEnumerator toPlantState03(float s){
		yield return new WaitForSeconds (s);
		Destroy (transform.GetChild (0).gameObject);
		GameObject pfb = Resources.Load("pumpkin_03") as GameObject;
		GameObject prefabInstance = Instantiate(pfb);
		prefabInstance.transform.parent = thisField.transform;
		prefabInstance.transform.position = new Vector3 (thisField.transform.position.x, thisField.transform.position.y+0.418f, thisField.transform.position.z);
		prefabInstance.transform.rotation = Quaternion.Euler( -90, 0, 0);

	}
}

