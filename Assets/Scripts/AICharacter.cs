using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICharacter : Character {
	private List<CellAction> _plan;
	private List<AStarNode> _nodePlan;
	private int _step;
	private int _currTransitionCell = 0;
	
	//public CellObject goalCell;

	private CellObject _currentStartCell;
	private CellObject _currentGoalCell;

	[SerializeField] private int _maxDepth = 100;

	private AStar _aStar;

	public bool won = false;

	void Awake () {
		_plan = new List<CellAction>();
		_step = -1;
		_aStar = new AStar(_maxDepth);
		_iAmRobot = true;
	}

	void Start () {
		this.gameObject.renderer.enabled = false;
		_currentStartCell = startCell;
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
		if (beatManager.beatNumber > -1) {
			if (_step == -1) {
				/*_nodePlan = _aStar.FindPath(startCell.cellNode,goalCell.cellNode);
				for (int i = 0; i < _nodePlan.Count-1; i++) {
					_plan.Add(CellAction.GetAction(_nodePlan[i] as CellNode,_nodePlan[i+1] as CellNode));
				}*/
				PlanOutPath();
				_step++;
			}
			if (_step < _plan.Count && beatManager.beatNumber >= 0){
				ExecuteCellAction(_plan[_step]);
				//Debug.LogError(_plan[_step].gameObject.name);
				_step++;
			}/* else {
				_currTransitionCell++;
				startCell = goalCell;
				goalCell = LevelManager.Instance.GetNextTransitionCell (_currTransitionCell);
				if (goalCell != null) {
					Debug.LogError("onto the next target!");
					_plan = new List<CellAction>();
					_nodePlan = _aStar.FindPath(startCell.cellNode,goalCell.cellNode);
					for (int i = 0; i < _nodePlan.Count-1; i++) {
						_plan.Add(CellAction.GetAction(_nodePlan[i] as CellNode,_nodePlan[i+1] as CellNode));
					}
					_step=0;
					if(_plan.Count > 0){
						ExecuteCellAction(_plan[_step]);
						//Debug.LogError(_plan[_step].gameObject.name);
						_step++;
					}
				} else {
					if (!won) {
						won = true;
						Debug.LogError("You Lose!");
					}
				}

			}*/
		}
	}

	public void PlanOutPath(){

		for (int i = 0; i < LevelManager.Instance.temporaryTotalPuzzleUnits; i++) {
			_currentGoalCell = LevelManager.Instance.GetNextTransitionCell (_currTransitionCell);
			//_currentGoalCell.cellNode.beat = -1;
			_currTransitionCell++;
			_nodePlan = _aStar.FindPath(_currentStartCell.cellNode,_currentGoalCell.cellNode);
			for (int j = 0; j < _nodePlan.Count-1; j++) {
				_plan.Add(CellAction.GetAction(_nodePlan[j] as CellNode,_nodePlan[j+1] as CellNode));
			}
			_currentStartCell = _currentGoalCell;
		}

	}
}
