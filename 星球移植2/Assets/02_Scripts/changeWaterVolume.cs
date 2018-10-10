using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeWaterVolume : MonoBehaviour {

	public float distance;
	public waterLevel waterLevel;
	public bool isPlanting;

	public Material meshRender = null;
	public Renderer rend;
	//public Texture texture;

	// Use this for initialization
	void Start () {
		waterLevel = gameObject.GetComponentInParent<waterLevel>();
		meshRender = Resources.Load("showWaterVolume/Materials/0水量") as Material;
		rend =  GetComponentInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//GetComponent<MeshRenderer>().materials[0] = meshRender;
		rend.sharedMaterial = meshRender;
		isPlanting = waterLevel.isPlanting;


		distance = Vector3.Distance (Camera.main.transform.position, transform.position);
		if (distance <= 10/*&&waterLevel.isPlanting==true/*&&waterLevel.water>=3*/) {
			gameObject.SetActive (true);
		} else {
			gameObject.SetActive (false);
		}

	}
}
