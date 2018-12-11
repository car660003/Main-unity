using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksPerformed : MonoBehaviour {
    public static string[] plant = { "南瓜", "小黃瓜", "紅蘿蔔","茄子","番茄" };
	[SerializeField]public Tasks m_task;

    public Text text;


	public int m_taskNum=1;

	private Tasks _task1;
	private Tasks _task2;
	private Tasks _task3;
	private Tasks _task4;
	private Tasks _task5;
	private Tasks _task6;
	private Tasks _task7;
	private Tasks _task8;
	private Tasks _task9;
	private Tasks _task10;


    private string[] _item1 = { plant[0]};
	private int[] _num1 = { 10};
	private string[] _item2 = { plant[1] };
    private int[] _num2 = { 10};
	private string[] _item3 = { plant[2] };
	private int[] _num3 = { 10};
	private string[] _item4 = { plant[3] };
    private int[] _num4 ={ 10};
	private string[] _item5 = { plant[4] };
	private int[] _num5 = { 10 };
	private string[] _item6;
	private int[] _num6;
	private string[] _item7;
	private int[] _num7;
	private string[] _item8;
	private int[] _num8;
	private string[] _item9;
	private int[] _num9;
	private string[] _item10;
	private int[] _num10;

	void Awake(){
		_task1 = new Tasks ();
		_task1.setTasks (_item1,_num1);
        m_task = _task1;
        
		_task2 = new Tasks ();
		_task2.setTasks (_item2,_num2);
        m_task = _task1;

        _task3 = new Tasks ();
		_task3.setTasks (_item3,_num3);
        m_task = _task1;
        _task4 = new Tasks ();
		_task4.setTasks (_item4,_num4);
        m_task = _task1;
        _task5 = new Tasks ();
		_task5.setTasks (_item5,_num5);
        m_task = _task1;
        _task6 = new Tasks ();
		_task6.setTasks (_item6,_num6);
		_task7 = new Tasks ();
		_task7.setTasks (_item7,_num7);
		_task8 = new Tasks ();
		_task8.setTasks (_item8,_num8);
		_task9 = new Tasks ();
		_task9.setTasks (_item9,_num9);
		_task10 = new Tasks ();
		_task10.setTasks (_item10,_num10);
        
    }

    void Update()
    {
        
       text.text = this.m_task._item[0].ToString() + ": " + this.m_task._numcount[0].ToString() + "/" + this.m_task._num[0].ToString();
		//text.text=pumpkin.Count.ToString();

    }

    public void addplant(string cc){
		m_task.addNum (cc);

		if (m_task.istask) {
			m_taskNum++;                //任務達成
            VegetableMainMessange.money += 100;
            StartCoroutine(waitforTask());
		}


	}

	IEnumerator waitforTask(){
		yield return new WaitForSeconds (2f);
        TasksNum(m_taskNum);
    }

	private void TasksNum (int _tasknum){
		switch(_tasknum){
		case 1:
			m_task = _task1;
			break;
		case 2:
			m_task = _task2;
			break;
            case 3:
                m_task = _task3;
                break;
            case 4:
                m_task = _task4;
                break;
            case 5:
                m_task = _task5;
                break;
            case 6:
                m_task = _task6;
                break;
            case 7:
                m_task = _task7;
                break;
            case 8:
                m_task = _task8;
                break;
            case 9:
                m_task = _task9;
                break;
            case 10:
                m_task = _task10;
                break;
            default:
                
                break;
		}
	}

	public void TasksNum1(){
		m_task = _task1;
	}
}
