﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {
	
	
	protected float horizontalGroundSpeed;
	protected float horizontalAirSpeed; // either we adjust friction, or just tune air vs. ground speed. Playing w/ friction may cause slip/slide issues
	protected float verticalSpeed;
	public float movementDuration;
	protected CellAction _currentAction;
	
	public CellObject startCell;
	
	protected CellObject _currentCell;
	protected CellObject _previousCell;
	protected bool _iAmRobot = false;
	protected int _nextTransitionCell = 0;

	protected Animator animator;

	// Use this for initialization
	protected virtual void Initialize () {
		BeatManager.Instance.Beat += BeatHandler;
		
		horizontalGroundSpeed = 30.0f; // 4:1 ratio seems solid
		horizontalAirSpeed = 4.0f;
		verticalSpeed = 30.0f;
		
		_currentAction = CellGridManager.Instance.wait;//new Action(new Vector3(), new Vector3(), 0.0f,this);
		//_currentCell = startCell.cellNode; TODO better way?

		Transform childSprite = gameObject.transform.GetChild(0);
		if (childSprite) animator = childSprite.GetComponent<Animator>();
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
			//MoveToInTime(rigidbody.position+new Vector3(cellAction.directionX*3.0f,cellAction.directionY*3.0f,0.0f),movementDuration);
			MoveToInTime(_currentCell.transform.position,movementDuration);
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

		// START ANIMATIONS
		if (animator) {
			// Jumping or falling
			if (verticalVelocity > 0) {
				animator.SetTrigger("StartJumping");
			}
			// Running
			else if (horizontalVelocity > 0) {
				animator.SetBool("Running", true);
			}
		}

		yield return new WaitForSeconds(duration*.8f);
		// PERFORM CELL BEHAVIOR
		// If ground
		if (_currentCell.transitionCell) {
			if (!_iAmRobot) {
				_currentCell.transitionCell = false;
				LevelManager.Instance.readyToSwitchUnits = true;
			}
		}
		if (_iAmRobot && _currentCell.transitionAICell) {
			_currentCell.transitionAICell = false;
			_nextTransitionCell++;
			if (_nextTransitionCell >= LevelManager.Instance.puzzleUnits.Count-1) {
				LevelManager.Instance.OnGameStateChange(GameState.Lose);
			}
		}
		if (_currentCell.cellType == CellType.Ground || _currentCell.cellType == CellType.MovingGround) {
			rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
			rigidbody.MovePosition(target);

			if (animator) {
				if (verticalVelocity > 0) {
					animator.SetTrigger("EndJumping");
				}
				// Running
				else if (horizontalVelocity > 0) {
					animator.SetBool("Running", false);
				}
			}

		} else if (_currentCell.cellType == CellType.Obstacle || _currentCell.cellType == CellType.MovingObstacle) {
			CellObject tempCell = _currentCell;
			if (_currentCell.returnToCheckpoint) {
				_currentCell = LevelManager.Instance.GetCheckPointCell();
			} else {
				_currentCell = _previousCell;
			}
			_previousCell = tempCell;
			MoveToInTime(_currentCell.gameObject.transform.position,duration); //TODO initiate on trigger not just at end?

			if (animator) {
				if (verticalVelocity > 0) {
					animator.SetTrigger("EndJumping");
				}
				// Running
				else if (horizontalVelocity > 0) {
					animator.SetBool("Running", false);
				}
			}

		} else {
			ExecuteCellAction(CellGridManager.Instance.down);

			if (animator) {
				if (verticalVelocity > 0) {
					animator.SetTrigger("EndJumping");
				}
				// Running
				else if (horizontalVelocity > 0) {
					animator.SetBool("Running", false);
				}
			}
		}
		// If obstacle
		// If empty
		// fall
	}
}
