using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSeed : MonoBehaviour {

	//public GameObject createPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void	createPumpkinSeed(){
		GameObject pfb = Resources.Load ("pumpkin/pumpkin_00_Seed") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		//prefabInstance.transform.parent = this.transform;//設為子物件
		//prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public void	createCucumberSeed(){
		GameObject pfb = Resources.Load ("cucumber/cucumber_00_Seed") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		//prefabInstance.transform.parent = this.transform;//設為子物件
		//prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public void	createCarrotSeed(){
		GameObject pfb = Resources.Load ("carrot/carrot_00_Seed") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		//prefabInstance.transform.parent = this.transform;//設為子物件
		//prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public void	createEggplantSeed(){
		GameObject pfb = Resources.Load ("eggplant/eggplant_00_Seed") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		//prefabInstance.transform.parent = this.transform;//設為子物件
		//prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	public void	createTomatoSeed(){
		GameObject pfb = Resources.Load ("tomato/tomato_00_Seed") as GameObject;//產生Pumpkin
		GameObject prefabInstance = Instantiate (pfb);
		//prefabInstance.transform.parent = this.transform;//設為子物件
		//prefabInstance.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

}
