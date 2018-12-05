using UnityEngine;  
using System.Collections;  

public class Move : MonoBehaviour
{  
	public AudioSource moving;

	/// <summary>  
	/// 手柄位置  
	/// </summary>  
	public SteamVR_TrackedObject tracked;  
	//public SteamVR_Controller.Device device;

	/// <summary>  
	/// 玩家  
	/// </summary>  
	public Transform player;  

	/// <summary>  
	/// 方向   
	/// </summary>  
	public Transform dic;  

	/// <summary>  
	/// 速度  
	/// </summary>  
	public float speed;  

	void Awake()  
	{  
		//获取手柄控制  
		tracked = GetComponent<SteamVR_TrackedObject>();  
		//device = SteamVR_Controller.Input ((int)tracked.index);
	}  

	// Use this for initialization  
	void Start () {  

	}  

	// Update is called once per frame  
	void FixedUpdate () {  
		var deviceright = SteamVR_Controller.Input((int)tracked.index);  

		if (Input.GetKey (KeyCode.T)) {
			Debug.Log("上");
		}
		//按下圆盘键  
		if (deviceright.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {  
			moving.volume = 1.0f;

			Vector2 cc = deviceright.GetAxis ();  

			float angle = VectorAngle (new Vector2 (1, 0), cc);  

			//下  
			if (angle > 45 && angle < 135) {  

				player.Translate (-dic.forward * Time.deltaTime * speed);  
			}  
			//上    
			else if (angle < -45 && angle > -135) {  
				//Debug.Log("上");  
				player.Translate (dic.forward * Time.deltaTime * speed);  
			}  
			//左    
			else if ((angle < 180 && angle > 135) || (angle < -135 && angle > -180)) {  
				//Debug.Log("左");  
				player.Translate (-dic.right * Time.deltaTime * speed);  
			}  
			//右    
			else if ((angle > 0 && angle < 45) || (angle > -45 && angle < 0)) {  
				//Debug.Log("右");  
				player.Translate (dic.right * Time.deltaTime * speed);  
			}  

		} else {
			moving.volume = 0.0f;
		}
	}  



	/// <summary>  
	/// 根据在圆盘才按下的位置，返回一个角度值  
	/// </summary>  
	/// <param name="from"></param>  
	/// <param name="to"></param>  
	/// <returns></returns>  
	float VectorAngle(Vector2 from, Vector2 to)  
	{  
		float angle;  
		Vector3 cross = Vector3.Cross(from, to);  
		angle = Vector2.Angle(from, to);  
		return cross.z > 0 ? -angle : angle;  
	}  

}  