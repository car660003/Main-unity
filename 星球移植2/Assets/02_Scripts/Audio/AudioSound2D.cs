using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSound2D : MonoBehaviour {

	AudioSource audioSource;
	public float thisVolume = 0.7F;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
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
	}
}
