using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weather : MonoBehaviour {
	
	[SerializeField]
	WeatherSTATUS weatherSTATUS;

	[SerializeField]
	ParticleSystem rainBasic;



	enum WeatherSTATUS{
		sunny,
		rain,
	}



	// Use this for initialization
	void Start () {
		weatherSTATUS = WeatherSTATUS.sunny;
		rainBasic = gameObject.GetComponentInChildren<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch(weatherSTATUS){
		case WeatherSTATUS.rain:
			rainBasic.GetComponentInChildren<ParticleSystem>().Play();
			break;

		case WeatherSTATUS.sunny:
			rainBasic.GetComponentInChildren<ParticleSystem>().Stop();
			break;
		}
	}
}
