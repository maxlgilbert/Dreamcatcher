using UnityEngine;
using System.Collections;

public class StartUIManager : MonoBehaviour {
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 40;

	public GameObject cloud01;
	public GameObject cloud02;
	public GameObject cloud03;
	public GameObject cloud04;

	int dir01;
	int dir02;
	int dir03;
	int dir04;

	void Start () {
		dir01 = -1;
		dir02 = 1;
		dir03 = -1;
		dir04 = -1;
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

	void FlipDir() {
		if (Mathf.Abs(cloud01.transform.position.x) > 10.8f) {
			dir01 = -1 * dir01;
			cloud01.transform.Translate(0, Random.Range(-1f, 1f), 0);
		}
		if (Mathf.Abs(cloud02.transform.position.x) > 10.8f) {
			dir02 = -1 * dir02;
			cloud02.transform.Translate(0, Random.Range(-1f, 1f), 0);
		}
		if (Mathf.Abs(cloud03.transform.position.x) > 10.8f) {
			dir03 = -1 * dir03;
			cloud03.transform.Translate(0, Random.Range(-1f, 1f), 0);
		}
		if (Mathf.Abs(cloud04.transform.position.x) > 10.8f) {
			dir04 = -1 * dir04;
			cloud04.transform.Translate(0, Random.Range(-1f, 1f), 0);
		}
	}

	void Update () {
		FlipDir();

		cloud01.transform.Translate(dir01 * 0.008f, 0, 0);
		cloud02.transform.Translate(dir02 * 0.006f, 0, 0);
		cloud03.transform.Translate(dir03 * 0.0035f, 0, 0);
		cloud04.transform.Translate(dir04 * 0.004f, 0, 0);
	}
}
