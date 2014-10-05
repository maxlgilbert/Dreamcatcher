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

	float horizontalSpeed;
	float verticalForce;
	bool isGrounded;

	void Start () {
		horizontalSpeed = 5.0f;
		verticalForce = 170.0f;
	}

	void Update () {
		// Do raycasts down to see if isGrounded should evaluate to true or not (left, middle, right of collider)
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

		if (Input.GetKey("up") && isGrounded) {
			gameObject.rigidbody.AddRelativeForce(new Vector3(0, verticalForce, 0));
		} else if (Input.GetKey("down")) {
			Debug.Log("down lol");
		} else if (Input.GetKey("left")) {
			rigidbody.velocity = new Vector3(-horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
		} else if (Input.GetKey("right")) {
			rigidbody.velocity = new Vector3(horizontalSpeed, rigidbody.velocity.y, rigidbody.velocity.z);
		}
	
	}
}
