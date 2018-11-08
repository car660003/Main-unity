using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(AudioSource))]
public class AudioSound3D : MonoBehaviour {

	AudioSource audioSource;

	public float distance;
	public bool playCheck = true;
	public float thisVolume = 0.7F;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		//isRain = WeatherMakerPrefab.GetComponent<WeatherMakerScript> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (VegetableMainMessange.isRain == true) {
			thisVolume = 0.3f;
			audioSource.volume = thisVolume;
		} else {
			thisVolume = 0.7f;
			audioSource.volume = thisVolume;
		}


		//判斷和玩家的距離小於maxDistance的話才撥放
		distance = Vector3.Distance (Camera.main.transform.position, transform.position);

		if (distance <= audioSource.maxDistance && playCheck == true) {
			audioSource.PlayOneShot (audioSource.clip, thisVolume);
			playCheck = false;
		} else if(distance > audioSource.maxDistance){
			playCheck = true;
		}
	}
}
