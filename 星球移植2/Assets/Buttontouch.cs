using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 給所有的按鈕創建碰撞體
/// </summary>
public class Buttontouch : MonoBehaviour {

	//public GameObject canvas;
	private Button[] _buttonArray;          //按鈕

	// Use this for initialization
	void Start () {
		_buttonArray = this.GetComponentsInChildren<Button>(true);            //取得所有的Button按鈕
		//給每個按钮創建一個碰撞體
		for(int i = 0; i < _buttonArray.Length; i++)
		{
			//判断按鈕是否有碰撞器
			if (_buttonArray[i].gameObject.GetComponent<Collider>() == null)
			{
				//創建碰撞器
				var buttonSize = _buttonArray[i].gameObject.GetComponent<RectTransform>().sizeDelta;
				BoxCollider button_BoxCollider = _buttonArray[i].gameObject.AddComponent<BoxCollider>();
				button_BoxCollider.size = new Vector3(buttonSize.x,buttonSize.y,2);
				button_BoxCollider.center =new Vector3(0, 0, 1);
			}
		}
	}
	void OnCollisionEnter()
	{
		　Debug.Log("A"); //碰到碰撞器時印出"A"
	}
}