using UnityEngine;
using System.Collections;

public class StartUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;

	void Start () {
		
	}

	void OnGUI() {
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Play")) {
			Debug.Log("Play");
			Application.LoadLevel("MainScene");
		}
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 60, BUTTON_WIDTH, BUTTON_HEIGHT), "About")) {
			Debug.Log("About");
		}
	}

	void Update () {
		
	}
}
