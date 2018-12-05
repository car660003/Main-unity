using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harvestVegetable : MonoBehaviour {
	[SerializeField] private TasksPerformed task;
	public waterLevel waterLevel;
	public bool ontri = false;

	public createPumpkin createPumpkin;
	public createCarrot createCarrot;
	public createTomato createTomato;
	public createCucumber createCucumber;
	public createEggplant createEggplant;



	// Use this for initialization
	void Start () {
        task = GameObject.FindGameObjectWithTag("task").GetComponent<TasksPerformed>();
		waterLevel = gameObject.GetComponentInParent<waterLevel>();

		//獲得土壤中的create植物.cs
		createPumpkin = gameObject.GetComponentInParent<createPumpkin>();
		createCarrot = gameObject.GetComponentInParent<createCarrot>();
		createTomato = gameObject.GetComponentInParent<createTomato>();
		createCucumber = gameObject.GetComponentInParent<createCucumber>();
		createEggplant = gameObject.GetComponentInParent<createEggplant>();

		/*if(createVeg_cs!=null){
			ontri = true;
		}*/

	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter(Collider sickle){
		
		if(sickle.tag == "sickle"){

			Destroy (createPumpkin); //刪除腳本
			Destroy (createCarrot); 
			Destroy (createTomato); 
			Destroy (createCucumber); 
			Destroy (createEggplant); 

				if(gameObject.tag=="pumpkin"){
					pumpkin.Count ++;
					task.addplant("南瓜");
                  //  task.addplant("新手");
                    waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);

				}else if(gameObject.tag=="cucumber"){
					cucumber.Count++;
                task.addplant("小黃瓜");
                waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);

				}else if(gameObject.tag=="carrot"){
					carrot.Count++;
                task.addplant("紅蘿蔔");
                waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);

				}else if(gameObject.tag=="eggplant"){
					eggplant.Count++;
                task.addplant("茄子");
                waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);

				}else if(gameObject.tag=="tomato"){
					tomato.Count++;
					task.addplant("番茄");
					waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);

				}else if(gameObject.tag=="dead"){
					waterLevel.water = 0;
					waterLevel.isPlanting = false;
					Destroy(gameObject);
				}	
		}
		


	}
}//Destroy (remainingSeconds_toString.GetComponents<createPumpkin>());
