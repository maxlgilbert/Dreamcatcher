using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICharacter : Character {
	private List<CellAction> _plan;
	private List<AStarNode> _nodePlan;
	private int _step;
	private int _currTransitionCell = 0;
	public float accuracy;
	public int mistakeFreeTime;
	
	//public CellObject goalCell;

	private CellObject _currentStartCell;
	private CellObject _currentGoalCell;

	[SerializeField] private int _maxDepth = 100;

	private AStar _aStar;

	public bool won = false;
	private bool _planning = false;

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
				//StartCoroutine("PlanOutPath");
				PlanOutPath();
				_step++;
			}
			if (!_planning && _step < _plan.Count && beatManager.beatNumber >= 0){
				float randF = Random.value;
				if (_step < mistakeFreeTime || randF <= accuracy) {
					ExecuteCellAction(_plan[_step]);
					//Debug.LogError(_plan[_step].gameObject.name);
					_step++;
				} else {
					//Debug.LogError("Made a new plan!");
					//ExecuteCellAction(CellGridManager.Instance.upLeft);
					_plan = new List<CellAction>();
					int numberOfWaits = Random.Range(1,mistakeFreeTime);
					for (int i = 0; i < numberOfWaits; i++){ 
						_plan.Add(CellGridManager.Instance.wait);
					}
					_step = 0;
					_currentStartCell = _currentCell;
					_currTransitionCell = _nextTransitionCell;
					//StartCoroutine("PlanOutPath");
					PlanOutPath();
				}
			}
			/* else {
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

	private void PlanOutPath(){
		_planning = true;
		_aStar = new AStar(_maxDepth);
		for (int i = _nextTransitionCell; i < LevelManager.Instance.puzzleUnits.Count-1; i++) {
			_currentGoalCell = LevelManager.Instance.GetNextTransitionCell (_currTransitionCell);
			_currTransitionCell++;
			//Debug.LogError(_currentStartCell.cellNode.ToString() +  _currentGoalCell.cellNode.ToString());
			_nodePlan = _aStar.FindPath(_currentStartCell.cellNode,_currentGoalCell.cellNode);
			for (int j = 0; j < _nodePlan.Count-1; j++) {
				_plan.Add(CellAction.GetAction(_nodePlan[j] as CellNode,_nodePlan[j+1] as CellNode));
			}
			_currentStartCell = _currentGoalCell;
			//yield return null; 
		}
		_planning = false;

	}
}
