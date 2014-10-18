using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;

	private bool paused;

	public GUIText timeElapsed;
	
	void Start() {
		Restart();
	}

	void Restart() {
		paused = false;
	}

	private void DrawPauseMenu() {
		// TODO: make sure it doesn't read any game inputs during pause other than P
		// TODO: figure out how to reload entire level (time and all) when wi-fi exists
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2, BUTTON_WIDTH, BUTTON_HEIGHT), "Resume")) {
			Debug.Log("Resume");
		}
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 50, BUTTON_WIDTH, BUTTON_HEIGHT), "Restart")) {
			Application.LoadLevel("MainScene");
			Time.timeScale = 1;
		}
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 100, BUTTON_WIDTH, BUTTON_HEIGHT), "Quit")) {
			Application.LoadLevel("StartScene");
			Time.timeScale = 1;
		}
	}

	void OnGUI() {
		if (paused) {
		DrawPauseMenu();
		}
	}

	void Update() {
		timeElapsed.text = Mathf.Round(Time.time).ToString();

		if (Input.GetKeyUp(KeyCode.P)) {
			paused = !paused;
			Time.timeScale = (paused) ? 0 : 1;
		}
	}
}
