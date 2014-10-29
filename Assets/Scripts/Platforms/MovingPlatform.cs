using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : Platform {
	private float percentOfBeat = .5f;
	private float _timeOfMovement;
	private Vector2 _velocity = new Vector2();
	private Vector3 initialPosition;
	private List<Vector2> _pathActions = new List<Vector2>();
	private List<CellObject> _pathNodes;
	private int _currentMove = 0;
	private int _totalMoves;
	public float yOffset;
	
	void Awake () {
		initialPosition = gameObject.transform.position;
	}

	public void InitializePath (List<Vector2> path) {
		_pathActions = path;
	}

	protected override void Initialize () {
		_timeOfMovement = .15f;//BeatManager.Instance.beatLength*percentOfBeat;
		_pathNodes = new List<CellObject>();
		_pathNodes.Add(currentCell);

	}

	protected override void SetCellTypes ()
	{
		int currBeat = 0;
		for (int i = 0; i < BeatManager.Instance.totalBeats; i++) {
			currentCell = _pathNodes[currBeat%_totalMoves];
			CellGridManager.Instance.SetCellTypeAtBeat(currentCell.x,currentCell.y,currBeat,this.cellType);
			//Debug.LogError("cell " + currentCell.x + "," + currentCell.y + " at beat: " + currBeat + " set to: " + this.cellType);
			currBeat++;
		}
		currentCell = _pathNodes[0];
	}
	protected override void BeatHandler (BeatManager beatManager)
	{
		if (beatManager.beatNumber == -1) {
			List<CellNode> affectedNodes = new List<CellNode>();
			affectedNodes.Add(currentCell.cellNode);
			for (int i =0; i < _pathActions.Count; i++) {
				int newXPosition = currentCell.x + (int)_pathActions[i].x;
				int newYPosition = currentCell.y + (int)_pathActions[i].y;
				currentCell = CellGridManager.Instance.GetCell(newXPosition,newYPosition);
				if (currentCell == null) {
					Debug.LogError("Can't move there: " + newXPosition + "," + newYPosition);
				}
				_pathNodes.Add(currentCell);
				affectedNodes.Add(currentCell.cellNode);
			}
			_totalMoves = _pathNodes.Count;
			currentCell = _pathNodes[0];
			SetCellTypes();
		} else if (BeatManager.Instance.beatNumber >= 0){
			StopCoroutine("Move");
			StartCoroutine("Move");
		}
	}
	/*IEnumerator Bounce() {
		gameObject.rigidbody.velocity = new Vector3(0,initialvelocity,0);
		yield return new WaitForSeconds(timeOfMovement/2.0f);
		gameObject.rigidbody.velocity = new Vector3(0,-initialvelocity,0);
		yield return new WaitForSeconds(timeOfMovement/2.0f);
		gameObject.rigidbody.velocity = new Vector3(0,0,0);
		gameObject.rigidbody.MovePosition(initialPosition);
	}*/

	
	IEnumerator Move() {
		_currentMove++;
		_currentMove %= _totalMoves;
		currentCell.cellType = CellType.Empty;
		Vector3 previousLocation = gameObject.transform.position;
		currentCell = _pathNodes[_currentMove];
		currentCell.cellType = this.cellType;
		Vector3 targetLocation = currentCell.gameObject.transform.position;
		targetLocation.y += yOffset;
		//targetLocation.y -= CellGridManager.Instance.yOffset;
		_velocity = (targetLocation - previousLocation)/_timeOfMovement;
		gameObject.rigidbody.velocity = new Vector3(_velocity.x,_velocity.y,0);
		yield return new WaitForSeconds(_timeOfMovement*.75f);
		gameObject.rigidbody.velocity = new Vector3(0,0,0);
		gameObject.rigidbody.MovePosition(targetLocation);
	}
}
