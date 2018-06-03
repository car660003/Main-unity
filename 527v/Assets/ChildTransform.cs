using UnityEngine;  
using System.Collections;  

public class ChildTransform : MonoBehaviour  
{  

	public Transform same;  

	// Use this for initialization  
	void Start () {  

	}  

	// Update is called once per frame  
	void FixedUpdate () {  

		transform.eulerAngles = new Vector3(0,same.eulerAngles.y,0);  

		transform.localPosition = new Vector3(same.localPosition.x, 0, same.localPosition.z);  
	}  
}  
