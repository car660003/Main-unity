using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Menuopcl : MonoBehaviour {

	public VRTK_ControllerEvents controllerEvents;
	public GameObject Menu;

	bool menuState = false;

	void onEnable()
	{
//		controllerEvents.StartMenuPressed+= ControllerEvents_StartMenuPressed;
//		controllerEvents.StartMenuReleased+= ControllerEvents_StartMenuReleased;
		controllerEvents.ButtonOnePressed+= ControllerEvents_ButtonOnePressed;
		controllerEvents.ButtonOneReleased += ControllerEvents_ButtonOneReleased;
	}

	void ControllerEvents_ButtonOneReleased (object sender, ControllerInteractionEventArgs e)
	{
		menuState = !menuState;
		Menu.SetActive (menuState);
	}

	void ControllerEvents_ButtonOnePressed (object sender, ControllerInteractionEventArgs e)
	{
		
	}


	void onDisable()
		{
//		controllerEvents.StartMenuPressed -= ControllerEvents_StartMenuPressed;	
//		controllerEvents.StartMenuReleased -= ControllerEvents_StartMenuReleased;
//		controllerEvents.ButtonTwoPressed -= ControllerEvents_ButtonTwoPressed;
//		controllerEvents.ButtonTwoReleased-= ControllerEvents_ButtonTwoReleased;
		controllerEvents.ButtonOnePressed-= ControllerEvents_ButtonOnePressed;
		controllerEvents.ButtonOneReleased-= ControllerEvents_ButtonOneReleased;
		}

/*	void ControllerEvents_StartMenuReleased (object sender, ControllerInteractionEventArgs e)
			{
			menuState = !menuState;
			Menu.SetActive (menuState);
			}

	void ControllerEvents_StartMenuPressed (object sender, ControllerInteractionEventArgs e)
	{
		
	}
*/

/*	void ControllerEvents_ButtonTwoReleased (object sender, ControllerInteractionEventArgs e)
	{
		menuState = !menuState;
		Menu.SetActive (menuState);
		Debug.Log ("A");
	}

	void ControllerEvents_ButtonTwoPressed (object sender, ControllerInteractionEventArgs e)
	{

	}
*/


}
