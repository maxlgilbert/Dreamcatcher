using UnityEngine;
using System.Collections;

public class BeatPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BeatManager.Instance.Beat += BeatHandler;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected virtual void BeatHandler (BeatManager beatManager) {
		Debug.Log("Beat fired!");
	}
}
