using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICharacter : Character {
	public List<CellAction> _plan;
	private int _step;

	void Awake () {
		//_plan = new List<CellAction>();
		_step = 0;
	}

	void Start () {
		Initialize();
	}

	protected override void Initialize ()
	{
		base.Initialize ();
	}
	// Update is called once per frame
	void Update () {
	
	}

	protected override void BeatHandler(BeatManager beatManager) {
		if (_step < _plan.Count){
			ExecuteCellAction(_plan[_step]);
			_step++;
		}
	}
}
