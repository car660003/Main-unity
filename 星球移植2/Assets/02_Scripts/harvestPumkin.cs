using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harvestPumkin : MonoBehaviour {
    private TasksPerformed task;
	public createPumpkin remainingSeconds_toString;
	// Use this for initialization
	void Start () {
        task = GameObject.FindGameObjectWithTag("task").GetComponent<TasksPerformed>();
		remainingSeconds_toString = gameObject.GetComponentInParent<createPumpkin>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider sickle){
		if(gameObject.tag!="dead"){
			if(sickle.gameObject.name == "sickle"){
				if(gameObject.tag=="pumpkin"){
					pumpkin.Count++;
					task.addplant("pumpkin");
					Destroy(this.gameObject);

				}else if(gameObject.tag=="cucumber"){
					cucumber.Count++;
					task.addplant("cucumber");
					Destroy(this.gameObject);

				}else if(gameObject.tag=="carrot"){
					carrot.Count++;
					task.addplant("carrot");
					Destroy(this.gameObject);

				}
			}
		}else if(gameObject.tag=="dead"){
			Destroy(this.gameObject);
		}


	}
}//Destroy (remainingSeconds_toString.GetComponents<createPumpkin>());
