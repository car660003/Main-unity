using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Talk : MonoBehaviour {
    public Flowchart talkFlowchart;
    public string onCollosionEnter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.CompareTag("玩家")) {
            Block targetBlock = talkFlowchart.FindBlock(onCollosionEnter);
            talkFlowchart.ExecuteBlock(targetBlock);
        }
    }
   
}
