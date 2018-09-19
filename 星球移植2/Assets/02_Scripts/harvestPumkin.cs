using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harvestPumkin : MonoBehaviour {
    private TasksPerformed task;
	// Use this for initialization
	void Start () {
        task = GameObject.FindGameObjectWithTag("task").GetComponent<TasksPerformed>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider sickle){
		if(sickle.gameObject.name == "sickle"){
			pumpkin.pumpkinCount++;
            task.addplant("pumpkin");
            Destroy(this.gameObject);
		}

	}
}
