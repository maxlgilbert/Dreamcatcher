﻿using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour {
	// Constants
	const float PADDING = 0.05f;
	const float BEAT_WINDOW = 1.0f;
	const float SECONDARY_KEY_WINDOW = 0.05f;

	float horizontalGroundSpeed;
	float horizontalAirSpeed; // either we adjust friction, or just tune air vs. ground speed. Playing w/ friction may cause slip/slide issues
	float verticalSpeed;
	bool isGrounded;
	bool isCollidingWithWall; // Used to fix sticky wall via friction. Thx Eric
	bool onBeat; // The beat window is open for BEAT_WINDOW seconds when this is true
	bool hasMovedOnBeat; // Whether or not we have moved once on the current beat

	string savedKey;
	bool waitingForSecondaryKey;

	private Camera _mainCamera;
	private Action _currentAction;

	void Start() {
		// TODO: put this in a game manager
		Physics.gravity = new Vector3(0, -30.0f, 0);

		BeatManager.Instance.Beat += BeatHandler;

		horizontalGroundSpeed = 16.0f; // 4:1 ratio seems solid
		horizontalAirSpeed = 4.0f;
		verticalSpeed = 12.0f;
		isGrounded = true;
		isCollidingWithWall = false;
		onBeat = false;
		hasMovedOnBeat = false;
		
		savedKey = "";
		waitingForSecondaryKey = false;

		this.gameObject.renderer.enabled = false;
		_mainCamera = Camera.main;
		_currentAction = new Action(new Vector3(), new Vector3(), 0.0f,this);
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
			//rigidbody.velocity = new Vector3(rigidbody.velocity.y, verticalSpeed, rigidbody.velocity.z);
			MoveToInTime(rigidbody.position + new Vector3(0.0f,3.6f,0.0f),.3f);
			hasMovedOnBeat = true;
			Debug.Log("up");
		} else if (savedKey == "left") {
			//rigidbody.velocity = new Vector3(-horizontalGroundSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
			MoveToInTime(rigidbody.position + new Vector3(-4.8f,0.0f,0.0f),.3f);
			hasMovedOnBeat = true;
			Debug.Log("left");
		} else if (savedKey == "right") {
			//rigidbody.velocity = new Vector3(horizontalGroundSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
			MoveToInTime(rigidbody.position + new Vector3(4.8f,0.0f,0.0f),.3f);
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
					//rigidbody.velocity = new Vector3(-horizontalAirSpeed, verticalSpeed, rigidbody.velocity.z);
					MoveToInTime(rigidbody.position + new Vector3(-4.8f,3.6f,0.0f),.3f);
					hasMovedOnBeat = true;
					Debug.Log("up left");
				}
				// If found right+up combo, close window and cancelInvoke. Then set velocity to up right
				else if ((Input.GetKey("right")  && savedKey == "up") || (Input.GetKey("up") && savedKey == "right")) {
					waitingForSecondaryKey = false;
					CancelInvoke("CloseSecondaryKeyWindow");
					//rigidbody.velocity = new Vector3(horizontalAirSpeed, verticalSpeed, rigidbody.velocity.z);
					MoveToInTime(rigidbody.position + new Vector3(4.8f,3.6f,0.0f),.3f);
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
	}

	private void CloseBeatWindow() {
		onBeat = false;
		hasMovedOnBeat = false;
	}
		
	private void BeatHandler(BeatManager beatManager) {
		Debug.Log("Daisy heard the beat!");
		onBeat = true;
		Invoke("CloseBeatWindow", BEAT_WINDOW);
        
    }

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Environment" && !isGrounded) {
			isCollidingWithWall = true;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Obstacle") {
			_currentAction.OnCollision();
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
