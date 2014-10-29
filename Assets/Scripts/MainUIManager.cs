using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;
	const float START_X = -20f; // arbitrary right now
	const float END_X = 100f; // arbitrary right now
	const float BEAT_INDICATOR_MAX_SCALE = 0.02f;

	private bool paused;
//	private Texture2D overlay;
	private Texture2D totalDistanceBar;
	private Texture2D totalDistanceBarStroke;
	public Texture2D distMeterBar;
	public Texture2D circleDaisy;
	public Texture2D circleDream;

	public Texture2D beatMeterBar;
	public Texture2D beatMeterTarget;
	public Texture2D beatMeterNote;

	public GameObject daisy;
	public GUIText timeElapsed;
	public GUIText distanceAway;
	public GUITexture beatIndicator;
	public GameObject overlayPlane;
	public GameObject overlayBottomBarPlane;

	void Start() {
		BeatManager.Instance.Beat += BeatHandler;
		Restart();
//		SetOverlayTexture();
	}

	void Restart() {
		paused = false;
	}

	private void DrawPauseMenu() {
		// TODO: make sure it doesn't read any game inputs during pause other than P
		// TODO: figure out how to reload entire level (time and all) when wi-fi exists
		if (GUI.Button(new Rect(Screen.width/2 - BUTTON_WIDTH/2, Screen.height/2, BUTTON_WIDTH, BUTTON_HEIGHT), "Resume")) {
			Debug.Log("Resume");
			paused = false;
			Time.timeScale = 1;
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

	private Color GetColorFrom256Scale(int r, int g, int b, float a) {
		return new Color(r/255f, g/255f, b/255f, a);
    }

//	private void SetOverlayTexture() {
//		int overlayWidth = Screen.width;
//		int overlayHeight = Screen.height/6;
//
//		overlay = new Texture2D(overlayWidth, overlayHeight);
//		Color[] colors = overlay.GetPixels();
//		for (int i = 0; i < colors.Length; i++) {
//			colors[i] = GetColorFrom256Scale(0, 0, 0, 0.5f);
//		}
//		overlay.SetPixels(colors);
//        overlay.Apply();
//	}

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
		GUI.BeginGroup(new Rect(Screen.width/2 + (Screen.width/2-(distMeterBar.width/3*2))/2, Screen.height/9 - 15, distMeterBar.width/3*2, circleDaisy.height/3*2));
			GUI.DrawTexture(new Rect (0, 7/3*2, distMeterBar.width/3*2, distMeterBar.height/3*2), distMeterBar);
			GUI.DrawTexture(new Rect (((daisy.transform.position.x-START_X)/(END_X-START_X)) * distMeterBar.width/3*2, 0, circleDaisy.width/3*2, circleDaisy.height/3*2), circleDaisy);
			GUI.DrawTexture(new Rect (distMeterBar.width/3*2 - 70, 0, circleDream.width/3*2, circleDream.height/3*2), circleDream); // abitrary X pos again since we don't have dream
		GUI.EndGroup();
	}

	private void DrawBeatMeter() {
		GUI.BeginGroup(new Rect(Screen.width/2 - (beatMeterBar.width/2), Screen.height/9 - 30, beatMeterBar.width, beatMeterBar.height));
			GUI.DrawTexture(new Rect(0, 0, beatMeterBar.width, beatMeterBar.height), beatMeterBar);
			GUI.DrawTexture(new Rect(20, 2, beatMeterTarget.width, beatMeterTarget.height), beatMeterTarget);
			GUI.DrawTexture(new Rect(50, 2, beatMeterNote.width, beatMeterNote.height), beatMeterNote);
		GUI.EndGroup();
	}

//	private void DrawOverlay() {
//		GUI.DrawTexture(new Rect(0, 0, overlay.width, overlay.height), overlay);
//	}

	private void PulseBeatIndicator() {
		int additionalScale = 20;
		Rect oldPix = beatIndicator.pixelInset;
		Rect newPix = new Rect(oldPix.x - additionalScale/2, oldPix.y - additionalScale/2, oldPix.width + additionalScale, oldPix.height + additionalScale);
		beatIndicator.pixelInset = newPix;
	}

	private void BeatHandler(BeatManager beatManager) {
		PulseBeatIndicator();
    }
    
    void OnGUI() {
//		DrawOverlay();
		if (paused) {
			DrawPauseMenu();
		}
		DrawDistanceMeter();
		DrawBeatMeter();
	}

	void Update() {
		timeElapsed.text = Mathf.Round(Time.time).ToString();
		distanceAway.text =  Mathf.RoundToInt(END_X - daisy.transform.position.x) + " m";
		overlayPlane.transform.position = new Vector3(Camera.main.transform.position.x, overlayPlane.transform.position.y, overlayPlane.transform.position.z);
		overlayBottomBarPlane.transform.position = new Vector3(Camera.main.transform.position.x, overlayBottomBarPlane.transform.position.y, overlayBottomBarPlane.transform.position.z);

		if (Input.GetKeyUp(KeyCode.P)) {
			paused = !paused;
			Time.timeScale = (paused) ? 0 : 1;
		}

		if (beatIndicator.pixelInset.width > 40) {
			Rect old = beatIndicator.pixelInset;
			beatIndicator.pixelInset = new Rect(old.x + 1, old.y + 1, old.width - 2, old.height - 2);
		}
	}
}
