using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStore : MonoBehaviour {


	public GameObject store;//商店menu

	public float open;

	public GameObject storeObj;
	public Material meshRender = null;
	public Renderer rend;
	// Use this for initialization
	void Start () {
		meshRender = Resources.Load("商店/Materials/StoreDark") as Material;
		rend =  storeObj.GetComponentInChildren<Renderer>();
		rend.material = meshRender;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider player){
		if (player.gameObject.name == "Camera (eye)") {
			meshRender = Resources.Load("商店/Materials/StoreLight") as Material;
			rend.material = meshRender;
			if(open>=2){
				store.SetActive(true);
			}
			open += Time.deltaTime;
		}
	}
	void OnTriggerExit(Collider player){
		if (player.gameObject.name == "Camera (eye)") {
			meshRender = Resources.Load("商店/Materials/StoreDark") as Material;
			rend.material = meshRender;
			store.SetActive(false);
			open = 0f;
		}
	}

}
