﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCharacter : Character {
	// Constants
	const float PADDING = 0.05f;
	const float BEAT_WINDOW = 1.0f;
	const float SECONDARY_KEY_WINDOW = 0.05f;
	bool isGrounded;
	bool isCollidingWithWall; // Used to fix sticky wall via friction. Thx Eric
	bool onBeat; // The beat window is open for BEAT_WINDOW seconds when this is true
	bool hasMovedOnBeat; // Whether or not we have moved once on the current beat

	string savedKey;
	bool waitingForSecondaryKey;

	private Camera _mainCamera;

	void Start() {
		// TODO: put this in a game manager
		Physics.gravity = new Vector3(0, -30.0f, 0);

		isGrounded = true;
		isCollidingWithWall = false;
		onBeat = false;
		hasMovedOnBeat = false;
		
		savedKey = "";
		waitingForSecondaryKey = false;

		this.gameObject.renderer.enabled = false;
		_mainCamera = Camera.main;

		Initialize();

	}

	protected override void Initialize ()
	{
		base.Initialize ();
		_currentCell = startCell;
	}

	// Do raycasts down to see if isGrounded should evaluate to true or not (left, middle, right of collider).
	// See the Debug ray drawing in	scene window (turns green on hit).
    private void CheckGroundedness() {
		int numRays = 3;
		float raycastPadding = 0.2f; // padding on leftmost and rightmost side to avoid buggy behavior against walls

		for (int i = 0; i < numRays; i++) {
			float originX = collider.bounds.center.x + ((i-1) * collider.bounds.extents.x);
			if (i == 0) originX += raycastPadding;
			else if (i == numRays - 1) originX -= raycastPadding;

			// Start the ray at the center of the object, because, if it were at the very bottom, it 
			// might not detect groundedness when resting on an object
			Vector3 origin = new Vector3 (originX, collider.bounds.center.y, collider.bounds.center.z);
			Vector3 direction = new Vector3(0, -1.0f, 0);

			// Perform raycast and determine groundedness. Draw Debug rays
			float rayLength = collider.bounds.extents.y + 1.0f;
			Ray ray = new Ray(origin, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, rayLength)) {
				float distanceToGround = hit.distance;
				isGrounded = (distanceToGround <= collider.bounds.extents.y + PADDING) ? true : false;
				Debug.DrawRay(ray.origin, new Vector3(0, -rayLength, 0), Color.green);
            } else {
				Debug.DrawRay(ray.origin, new Vector3(0, -rayLength, 0), Color.red);
            }
        };
	}

	// Callback at the end of the secondary key window. If a key combo was not detected for the duration of the 
	// secondary key window period (up+left/up+right), close the window and perform the action of the saved key
	// This seems annoying but we need to do this to truly ensure 1 movement per beat. Otherwise other keypresses
	// will leak through during the beat window.
	private void CloseSecondaryKeyWindow() {
		waitingForSecondaryKey = false;
		if (savedKey == "up") {
			//List<AStarNode> neighbors = CellGridManager.Instance.up.TryAction(_currentCell);
			//if (neighbors.Count > 0) {
//				_currentCell = neighbors[0] as CellNode;
//				MoveToInTime(rigidbody.position + new Vector3(0.0f,3,0.0f),.3f);
				ExecuteCellAction(CellGridManager.Instance.up);
				hasMovedOnBeat = true; //IN HERE????
				Debug.Log("up");
		} else if (savedKey == "left") {
			//List<AStarNode> neighbors = CellGridManager.Instance.left.TryAction(_currentCell);
			//if (neighbors.Count > 0) {
//				_currentCell = neighbors[0] as CellNode;
//				MoveToInTime(rigidbody.position + new Vector3(-3.0f,0.0f,0.0f),.3f);
				ExecuteCellAction(CellGridManager.Instance.left);
				hasMovedOnBeat = true;
				Debug.Log("left");
		} else if (savedKey == "right") {
			//List<AStarNode> neighbors = CellGridManager.Instance.right.TryAction(_currentCell);
			//if (neighbors.Count > 0) {
//				_currentCell = neighbors[0] as CellNode;
//				MoveToInTime(rigidbody.position + new Vector3(3.0f,0.0f,0.0f),.3f);
				ExecuteCellAction(CellGridManager.Instance.right);
				hasMovedOnBeat = true;
				Debug.Log("right");
		}

		savedKey = "";
	}

	// Handles movement with arrow keys
	private void CheckInput() {

		if (onBeat && !hasMovedOnBeat) {

			// This means up/left/right was pressed earlier, and our secondary key window is open
			if (waitingForSecondaryKey) {

				// If found left+up combo, close window and cancelInvoke. Then set velocity to up left
				if ((Input.GetKey("left") && savedKey == "up") || (Input.GetKey("up") && savedKey == "left") ) {
					waitingForSecondaryKey = false;
					CancelInvoke("CloseSecondaryKeyWindow");
//						_currentCell = neighbors[0] as CellNode;
//						MoveToInTime(rigidbody.position + new Vector3(-3.0f,3.0f,0.0f),.3f);
						ExecuteCellAction(CellGridManager.Instance.upLeft);
						hasMovedOnBeat = true;
						Debug.Log("up left");
				}
				// If found right+up combo, close window and cancelInvoke. Then set velocity to up right
				else if ((Input.GetKey("right")  && savedKey == "up") || (Input.GetKey("up") && savedKey == "right")) {
					waitingForSecondaryKey = false;
					CancelInvoke("CloseSecondaryKeyWindow");
//						_currentCell = neighbors[0] as CellNode;
//						MoveToInTime(rigidbody.position + new Vector3(3.0f,3.0f,0.0f),.3f);
						ExecuteCellAction(CellGridManager.Instance.upRight);
						hasMovedOnBeat = true;
						Debug.Log("up right");
				}

			} else if (Input.GetKey("up")){// && isGrounded) {
				waitingForSecondaryKey = true;
				savedKey = "up";
				Invoke("CloseSecondaryKeyWindow", SECONDARY_KEY_WINDOW);

			} else if (Input.GetKey("left") && !isCollidingWithWall) {
				waitingForSecondaryKey = true;
				savedKey = "left";
				Invoke("CloseSecondaryKeyWindow", SECONDARY_KEY_WINDOW);

			} else if (Input.GetKey("right") && !isCollidingWithWall) {
				waitingForSecondaryKey = true;
				savedKey = "right";
				Invoke("CloseSecondaryKeyWindow", SECONDARY_KEY_WINDOW);
	        }
		}
		if (!onBeat && ( Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up"))) {
			BadMoveAnimation();
		}

		// A debug key input
		if (Input.GetKeyUp(KeyCode.Q)) {
			BadMoveAnimation();
		}
	}

	private void CloseBeatWindow() {
		onBeat = false;
		hasMovedOnBeat = false;
	}
		
	protected override void BeatHandler(BeatManager beatManager) {
		//Debug.Log("Daisy heard the beat!");
		onBeat = true;
		Invoke("CloseBeatWindow", BEAT_WINDOW);
        
    }
	
	private void BadMoveAnimation() {
		iTween.ShakePosition(this.gameObject, new Vector3(0.3f, 0), 0.18f);
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Environment" && !isGrounded) {
			isCollidingWithWall = true;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Obstacle") {
			//_currentAction.OnCollision();
		} else if (col.gameObject.tag == "Ground") {
			MoveToInTime(rigidbody.position,0.0f);
		}
	}

	void OnCollisionExit() {
		isCollidingWithWall = false;
	}

	void Update() {
		CheckGroundedness();
		CheckInput();
		_mainCamera.transform.position = new Vector3(gameObject.transform.position.x,
		                                             _mainCamera.transform.position.y,
		                                             _mainCamera.transform.position.z);
	}
}
