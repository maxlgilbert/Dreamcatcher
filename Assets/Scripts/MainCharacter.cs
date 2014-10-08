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
	bool onBeat;

	void Start () {
		BeatManager.Instance.Beat += BeatHandler;

		horizontalSpeed = 5.0f;
		verticalForce = 150.0f;
		isGrounded = true;
		isCollidingWithWall = false;
		onBeat = false;

		this.gameObject.renderer.enabled = false;
	}

	// Do raycasts down to see if isGrounded should evaluate to true or not (left, middle, right of collider)
    private void CheckGroundedness() {
		int numRays = 3;
		float raycastPadding = 0.2f;
		for (int i = 0; i < numRays; i++) {
			float originX = collider.bounds.center.x + ((i-1) * collider.bounds.extents.x);
			if (i == 0) originX += raycastPadding;
			else if (i == numRays - 1) originX -= raycastPadding;
			Vector3 origin = new Vector3 (originX, collider.bounds.center.y);
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

		if (onBeat) {
			if (Input.GetKey("up") && isGrounded) {
				gameObject.rigidbody.AddRelativeForce(new Vector3(0, verticalForce, 0));
				Debug.Log("up");

			} else if (Input.GetKey("down")) {
				Debug.Log("down lol");
			} else if (Input.GetKey("left") && !isCollidingWithWall) {
				rigidbody.velocity = new Vector3(-horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
			} else if (Input.GetKey("right") && !isCollidingWithWall) {
				rigidbody.velocity = new Vector3(horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
	        }
		}
	}

	private void CloseBeatWindow() {
		onBeat = false;
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
	}
}
