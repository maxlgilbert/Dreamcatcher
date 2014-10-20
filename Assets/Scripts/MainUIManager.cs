﻿using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;
	const float START_X = -20f; // arbitrary right now
	const float END_X = 100f; // arbitrary right now

	private bool paused;
	private Texture2D totalDistanceBar;
	private Texture2D totalDistanceBarStroke;
	public Texture2D distMeterBar;
	public Texture2D circleDaisy;
	public Texture2D circleDream;

	public GameObject daisy;
	public GUIText timeElapsed;
	public GUIText distanceAway;

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

	private Color GetColorFrom256Scale(int r, int g, int b) {
		return new Color(r/255f, g/255f, b/255f);
	}

	// Not used right now but may use later
	private void SetDistanceMeterTextures() {
		int TDBWidth = 250;
		int TDBHeight = 7;

		totalDistanceBar = new Texture2D(TDBWidth, TDBHeight);
		Color[] colorsTDB = totalDistanceBar.GetPixels();
		for (int i = 0; i < colorsTDB.Length; i++) {
			colorsTDB[i] = GetColorFrom256Scale(2, 61, 110);
		}
		totalDistanceBar.SetPixels(colorsTDB);
		totalDistanceBar.Apply();

		totalDistanceBarStroke = new Texture2D(TDBWidth + 4, TDBHeight + 4);
		Color[] colorsTDBS = totalDistanceBarStroke.GetPixels();
		for (int i = 0; i < colorsTDBS.Length; i++) {
			colorsTDBS[i] = GetColorFrom256Scale(1, 1, 1);
		}
		totalDistanceBarStroke.SetPixels(colorsTDBS);
		totalDistanceBarStroke.Apply();

	}

	private void DrawDistanceMeter() {
		GUI.BeginGroup(new Rect(Screen.width/2 + (Screen.width/2-(distMeterBar.width/3*2))/2, Screen.height/9 - 5, distMeterBar.width/3*2, circleDaisy.height/3*2));
			GUI.DrawTexture(new Rect (0, 7/3*2, distMeterBar.width/3*2, distMeterBar.height/3*2), distMeterBar);
			GUI.DrawTexture(new Rect (((daisy.transform.position.x-START_X)/(END_X-START_X)) * distMeterBar.width/3*2, 0, circleDaisy.width/3*2, circleDaisy.height/3*2), circleDaisy);
			GUI.DrawTexture(new Rect (distMeterBar.width/3*2 - 70, 0, circleDream.width/3*2, circleDream.height/3*2), circleDream); // abitrary X pos again since we don't have dream
		GUI.EndGroup();
	}

	void OnGUI() {
		if (paused) {
			DrawPauseMenu();
		}
		DrawDistanceMeter();
	}

	void Update() {
		timeElapsed.text = Mathf.Round(Time.time).ToString();
		distanceAway.text =  Mathf.RoundToInt(END_X - daisy.transform.position.x) + " m";

		if (Input.GetKeyUp(KeyCode.P)) {
			paused = !paused;
			Time.timeScale = (paused) ? 0 : 1;
		}
	}
}