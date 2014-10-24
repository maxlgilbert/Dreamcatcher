using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : Platform {
	public float timeOfMovement = .5f;
	private Vector2 _velocity = new Vector2();
	private Vector3 initialPosition;
	public List<Vector2> pathActions = new List<Vector2>();
	private List<CellObject> _pathNodes;
	private int _currentMove = 0;
	private int _totalMoves;
	
	void Awake () {
		initialPosition = gameObject.transform.position;
	}

	protected override void Initialize () {
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
			for (int i =0; i < pathActions.Count; i++) {
				int newXPosition = currentCell.x + (int)pathActions[i].x;
				int newYPosition = currentCell.y + (int)pathActions[i].y;
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
		} else {
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
		Vector2 previousLocation = currentCell.location;
		currentCell = _pathNodes[_currentMove];
		currentCell.cellType = this.cellType;
		_velocity = (currentCell.location - previousLocation)/timeOfMovement;
		gameObject.rigidbody.velocity = new Vector3(_velocity.x,_velocity.y,0);
		yield return new WaitForSeconds(timeOfMovement);
		gameObject.rigidbody.velocity = new Vector3(0,0,0);
		//gameObject.rigidbody.MovePosition(initialPosition);
	}
}
