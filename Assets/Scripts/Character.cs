using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	
	protected float horizontalGroundSpeed;
	protected float horizontalAirSpeed; // either we adjust friction, or just tune air vs. ground speed. Playing w/ friction may cause slip/slide issues
	protected float verticalSpeed;
	protected Action _currentAction;
	
	public CellObject startCell;
	
	protected CellNode _currentCell;
	// Use this for initialization
	protected virtual void Initialize () {
		BeatManager.Instance.Beat += BeatHandler;
		
		horizontalGroundSpeed = 16.0f; // 4:1 ratio seems solid
		horizontalAirSpeed = 4.0f;
		verticalSpeed = 12.0f;
		
		_currentAction = new Action(new Vector3(), new Vector3(), 0.0f,this);
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
			_currentCell = startCell.cellNode; // TODO
		}
		_currentCell = CellGridManager.Instance.GetCell(_currentCell.x+cellAction.directionX,_currentCell.y+cellAction.directionY).cellNode;
		MoveToInTime(rigidbody.position+new Vector3(cellAction.directionX*3.0f,cellAction.directionY*3.0f,0.0f),.3f);
	}
	
	public void MoveToInTime (Vector3 target, float duration){
		StopCoroutine("MoveToInTimeCoroutine");
		_currentAction.returnPt = rigidbody.position;
		_currentAction.endPt = target;
		_currentAction.duration = duration;
		StartCoroutine(MoveToInTimeCoroutine(target,duration));
	}
	
	IEnumerator MoveToInTimeCoroutine(Vector3 target, float duration){
		float horizontalVelocity = 0.0f;
		float verticalVelocity = 0.0f;
		horizontalVelocity = (target.x - rigidbody.position.x)/duration;
		verticalVelocity = (target.y - rigidbody.position.y)/duration;
		rigidbody.velocity = new Vector3(horizontalVelocity, verticalVelocity, rigidbody.velocity.z);
		yield return new WaitForSeconds(duration);
		rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
		rigidbody.MovePosition(target);
	}
}
