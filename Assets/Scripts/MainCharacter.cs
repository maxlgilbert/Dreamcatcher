using UnityEngine;
using System.Collections;

/* Things our main character needs:
 * speed
 * movement
 * groundedness
 * constrained movements to the beat
 */

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
		verticalForce = 170.0f;
		isGrounded = true;
		isCollidingWithWall = false;
		onBeat = false;
	}

	// Do raycasts down to see if isGrounded should evaluate to true or not (left, middle, right of collider)
    private void CheckGroundedness() {
		for (int i = 0; i < 3; i++) {
			Vector3 origin = new Vector3 (collider.bounds.center.x + ((i-1) * collider.bounds.extents.x), collider.bounds.center.y);
			Vector3 direction = new Vector3(0, -1.0f, 0);
			Ray ray = new Ray(origin, direction);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, collider.bounds.extents.y + 1.0f)) {
				float distanceToGround = hit.distance;
				isGrounded = (distanceToGround <= collider.bounds.extents.y + PADDING) ? true : false;
				Debug.DrawRay(ray.origin, ray.direction, Color.green);
            } else {
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
            }
        };
	}

	// Handles movement with arrow keys
	private void CheckInput() {

		if (onBeat) {
			if (Input.GetKey("up") && isGrounded) {
				gameObject.rigidbody.AddRelativeForce(new Vector3(0, verticalForce, 0));
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
