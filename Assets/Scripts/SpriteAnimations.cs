using UnityEngine;
using System.Collections;

public class SpriteAnimations : MonoBehaviour {

	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (animator) {

			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				animator.SetTrigger("StartJumping");
			}
			if (Input.GetKeyUp(KeyCode.UpArrow)) {
				animator.SetTrigger("EndJumping");
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				animator.SetBool("Running", true);
			}
			if (Input.GetKeyUp(KeyCode.RightArrow)) {
				animator.SetBool("Running", false);
			}
		}

	}
}
