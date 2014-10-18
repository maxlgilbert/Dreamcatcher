using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;

	private bool paused;

	public GUIText timeElapsed;
	
	void Start() {
	
	}

	private void DrawPauseMenu() {
		// TODO: make sure it doesn't read any game inputs during pause other than P
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2, BUTTON_WIDTH, BUTTON_HEIGHT), "Resume")) {
			Debug.Log("Resume");
		}
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 50, BUTTON_WIDTH, BUTTON_HEIGHT), "Restart")) {
			Application.LoadLevel("MainScene");
		}
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2 + 100, BUTTON_WIDTH, BUTTON_HEIGHT), "Quit")) {
			Application.LoadLevel("StartScene");
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
