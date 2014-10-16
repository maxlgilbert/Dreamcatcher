using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour {
	// Constants
	const float PADDING = 0.05f;
	const float BEAT_WINDOW = 1.0f;

	float horizontalSpeed;
	float verticalForce;
	bool isGrounded;
	bool isCollidingWithWall; // Used to fix sticky wall via friction. Thx Eric
	bool onBeat; // The beat window is open for BEAT_WINDOW seconds when this is true
	bool hasMovedOnBeat; // Whether or not we have moved once on the current beat

	private Camera _mainCamera;

	void Start() {
		BeatManager.Instance.Beat += BeatHandler;

		horizontalSpeed = 5.0f;
		verticalForce = 8.0f;
		isGrounded = true;
		isCollidingWithWall = false;
		onBeat = false;

		this.gameObject.renderer.enabled = false;
		_mainCamera = Camera.main;
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

	// Handles movement with arrow keys
	private void CheckInput() {

		if (onBeat && !hasMovedOnBeat) {
			if (Input.GetKey("up") && isGrounded) {
				rigidbody.velocity = new Vector3(rigidbody.velocity.y, verticalForce, rigidbody.velocity.z);
				hasMovedOnBeat = true;
				Debug.Log("up");
			} else if (Input.GetKey("down")) {
				Debug.Log("down lol");
			} else if (Input.GetKey("left") && !isCollidingWithWall) {
				rigidbody.velocity = new Vector3(-horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
				hasMovedOnBeat = true;
			} else if (Input.GetKey("right") && !isCollidingWithWall) {
				rigidbody.velocity = new Vector3(horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
				hasMovedOnBeat = true;
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
