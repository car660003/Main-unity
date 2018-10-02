using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Menuopcl : MonoBehaviour {

	public VRTK_ControllerEvents controllerEvents;
	public GameObject MenuObj;

	bool menuState = false;

	void OnEnable()
	{
		controllerEvents.ButtonTwoReleased +=  ControllerEvents_ButtonTwoReleased;
	}

	void OnDisable()
	{
		controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
	}




	void ControllerEvents_ButtonTwoReleased (object sender, ControllerInteractionEventArgs e)
	{
		menuState = !MenuObj.activeSelf;
		MenuObj.SetActive (menuState);
	}




}
