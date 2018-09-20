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
		if(sickle.gameObject.name == "sickle"){
			pumpkin.pumpkinCount++;
            task.addplant("pumpkin");
			//Destroy (remainingSeconds_toString.GetComponents<createPumpkin>());
            Destroy(this.gameObject);
		}

	}
}
