using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	
	
	protected float horizontalGroundSpeed;
	protected float horizontalAirSpeed; // either we adjust friction, or just tune air vs. ground speed. Playing w/ friction may cause slip/slide issues
	protected float verticalSpeed;
	protected CellAction _currentAction;
	
	public CellObject startCell;
	
	protected CellObject _currentCell;
	protected CellObject _previousCell;
	// Use this for initialization
	protected virtual void Initialize () {
		BeatManager.Instance.Beat += BeatHandler;
		
		horizontalGroundSpeed = 16.0f; // 4:1 ratio seems solid
		horizontalAirSpeed = 4.0f;
		verticalSpeed = 12.0f;
		
		_currentAction = CellGridManager.Instance.wait;//new Action(new Vector3(), new Vector3(), 0.0f,this);
		//_currentCell = startCell.cellNode; TODO better way?
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	protected virtual void BeatHandler(BeatManager beatManager) {
		Debug.Log("Beat!");
		
	}

	public void ExecuteCellAction (CellAction cellAction) {
		if (_currentCell == null) {
			_currentCell = startCell; // TODO
			_previousCell = startCell;
		}
		
		int x = _currentCell.x + cellAction.directionX;
		int y = _currentCell.y + cellAction.directionY;
		CellObject neighbor = CellGridManager.Instance.GetCell(x,y);
		if (neighbor!=null) {
			_previousCell = _currentCell;
			_currentCell = neighbor;
			//_currentCell = CellGridManager.Instance.GetCell(_currentCell.x+cellAction.directionX,_currentCell.y+cellAction.directionY).cellNode;
			MoveToInTime(rigidbody.position+new Vector3(cellAction.directionX*3.0f,cellAction.directionY*3.0f,0.0f),.3f);
		}
	}
	
	public void MoveToInTime (Vector3 target, float duration){
		StopCoroutine("MoveToInTimeCoroutine");
		//_currentAction.returnPt = rigidbody.position;
		//_currentAction.endPt = target;
		//_currentAction.duration = duration;
		StartCoroutine(MoveToInTimeCoroutine(target,duration));
	}
	
	IEnumerator MoveToInTimeCoroutine(Vector3 target, float duration){
		Vector3 originalLocation = rigidbody.position;
		float horizontalVelocity = 0.0f;
		float verticalVelocity = 0.0f;
		horizontalVelocity = (target.x - rigidbody.position.x)/duration;
		verticalVelocity = (target.y - rigidbody.position.y)/duration;
		rigidbody.velocity = new Vector3(horizontalVelocity, verticalVelocity, rigidbody.velocity.z);
		yield return new WaitForSeconds(duration);
		// PERFORM CELL BEHAVIOR
		// If ground
		if (_currentCell.cellType == CellType.Empty || _currentCell.cellType == CellType.Ground) {
			rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
			rigidbody.MovePosition(target);
		} else if (_currentCell.cellType == CellType.Obstacle) {
			CellObject tempCell = _currentCell;
			_currentCell = _previousCell;
			_previousCell = tempCell;
			MoveToInTime(originalLocation,duration); //TODO initiate on trigger not just at end?
		}
		// If obstacle
		// If empty
		// fall
	}
}
