using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingMovie : MonoBehaviour {

	public VideoPlayer vp;
	public GameObject endingPlane;
	public bool playOneShot = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(pumpkin.Count>0&&cucumber.Count>0&&carrot.Count>0&&eggplant.Count>0&&tomato.Count>0){
			if(playOneShot==false){
				endingPlane.SetActive (true);
				vp.Play();
				playOneShot = true;
				StartCoroutine (close());
			}
		}
		/*if(!vp.isPlaying){
			endingPlane.SetActive (false);
		}*/
		if(Input.GetKeyDown(KeyCode.Escape)){
			playOneShot = false;
		}
	}
	IEnumerator close(){
		yield return new WaitForSeconds(60);
		endingPlane.SetActive (false);
	}
}
