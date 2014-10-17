using UnityEngine;
using System.Collections;

public class Action {
	public Vector3 returnPt;
	public Vector3 endPt;
	public float duration;
	public MainCharacter character;
	public Action (Vector3 _returnPt, Vector3 _endPt, float _duration, MainCharacter _character) {
		returnPt = _returnPt;
		endPt = _endPt;
		duration = _duration;
		character = _character;
	}
	public void OnCollision  () {
		character.MoveToInTime(returnPt,duration);
	}
}
