using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICharacter : Character {
	public List<CellAction> _plan;
	private List<AStarNode> _nodePlan;
	private int _step;
	
	public CellObject goalCell;

	[SerializeField] private int _maxDepth = 100;

	private AStar _aStar;

	void Awake () {
		_plan = new List<CellAction>();
		_step = -1;
		_aStar = new AStar(_maxDepth);
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
		if (_step == -1) {
			_nodePlan = _aStar.FindPath(startCell.cellNode,goalCell.cellNode);
			for (int i = 0; i < _nodePlan.Count-1; i++) {
				_plan.Add(CellAction.GetAction(_nodePlan[i] as CellNode,_nodePlan[i+1] as CellNode));
			}
			_step++;
		}
		if (_step < _plan.Count){
			ExecuteCellAction(_plan[_step]);
			_step++;
		}
	}
}
