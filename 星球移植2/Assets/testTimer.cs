using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class testTimer : MonoBehaviour {

	public Text dateTimeText;

	void Update () {
		dateTimeText.text = DateTime.Now.ToString ();
	}
}