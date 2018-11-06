using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks {

	public string[] _item;
	public int[] _num;
	public int[] _numcount = new int[9];
	public bool istask = false;

	public void setTasks(string[] item, int[] num){
		_item = item;
		_num = num;

    }

	private void isOverTask(){
		bool overTask = true;
		for (int i = 0; i < _num.Length; i++) {
			if (_numcount [i] < _num [i]) {
				overTask = false;
				break;
			}
		}
		if (overTask == true) {
            Debug.Log("next task");
			istask = true;
		}
	}

	public void addNum(string cc){
        Debug.Log("addNum plant: " + cc);
        for (int i = 0; i < _item.Length; i++) {
			if (cc == _item [i]) {
				if (_numcount [i] < _num[i])
                {
					_numcount [i]++;
                    Debug.Log(_numcount[i]);
                }
                
			}
		}
		isOverTask ();
	}
}
