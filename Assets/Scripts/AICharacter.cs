using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICharacter : Character {
	private List<CellAction> _plan;

	void Awake () {
		_plan = new List<CellAction>();
	}
	// Update is called once per frame
	void Update () {
	
	}

	protected override void BeatHandler(BeatManager beatManager) {

	}
}
