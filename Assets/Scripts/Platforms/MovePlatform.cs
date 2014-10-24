using UnityEngine;
using System.Collections;

public class MovePlatform : BeatPlatform {
	public float timeOfMovement = 1.0f;
	public float initialvelocity = 10.0f;
	private Vector3 initialPosition;

	void Awake () {
		initialPosition = gameObject.transform.position;
	}
	protected override void BeatHandler (BeatManager beatManager)
	{
		StartCoroutine("Bounce");
	}
	IEnumerator Bounce() {
		gameObject.rigidbody.velocity = new Vector3(0,initialvelocity,0);
		yield return new WaitForSeconds(timeOfMovement/2.0f);
		gameObject.rigidbody.velocity = new Vector3(0,-initialvelocity,0);
		yield return new WaitForSeconds(timeOfMovement/2.0f);
		gameObject.rigidbody.velocity = new Vector3(0,0,0);
		gameObject.rigidbody.MovePosition(initialPosition);
	}
}
