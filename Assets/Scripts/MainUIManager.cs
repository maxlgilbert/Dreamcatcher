using UnityEngine;
using System.Collections;

public class MainUIManager : MonoBehaviour {
	public GUIText timeElapsed;


	void Start () {
	
	}

	void Update () {
		timeElapsed.text = Mathf.Round(Time.time).ToString();
	}
}
