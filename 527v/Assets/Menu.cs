using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class Menu: MonoBehaviour
{
	public GameObject[] ui; //所有UI面板
	private bool toggle = false;

	void Start()
	{
		//使用VRTK的Plugin並且using VRTK;這個Namespace
		if (GetComponent<VRTK_ControllerEvents>() == null)
		{
			this.enabled = false;
			return;
		}
		GetComponent<VRTK_ControllerEvents>().StartMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
		GetComponent<VRTK_ControllerEvents>().StartMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuReleased);
	}

	void Update()
	{
	}

	private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
	{
	}

	//當按下放開Vive的Pad前方的Nemu按鈕
	private void DoApplicationMenuReleased(object sender, ControllerInteractionEventArgs e)
	{
		ToggleUI(!toggle);
	}

	public void ToggleUI(bool active)
	{
		foreach (GameObject go in ui)
			go.SetActive(active);
		toggle = active;
	}
}