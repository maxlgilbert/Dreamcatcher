using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class iSSortedList {
	public iSSortedList () {
		_internalList = new List<AStarNode>();
	}
	private List<AStarNode> _internalList;

	public int Count () {
		return _internalList.Count;
	}
	public void Add (AStarNode element) {
		int i = 0;
		for (i = 0; i < _internalList.Count; i++) {
			bool broke = false;
			if(element.fValue < _internalList[i].fValue) {
				broke = true;
				break;
			}
			if (broke) {
				Debug.LogError("NOOO");
			}
		}
		_internalList.Insert(i,element);
		string printIt = "";
		for (int j = 0; j <_internalList.Count; j++) {
			printIt += _internalList[j].fValue + ", ";
		}
	}

	public AStarNode Pop() {
		if (_internalList.Count != 0) {
			AStarNode top =  _internalList[0];
			_internalList.RemoveAt(0);
			return top;
		}
		return null;
	}


}
