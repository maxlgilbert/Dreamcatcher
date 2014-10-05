using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour {

	private static BeatManager instance;
	
	public static BeatManager Instance
	{
		get 
		{
			return instance;
		}
	}
	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	public delegate void BeatEventHandler(BeatManager beatManager);
	public event BeatEventHandler Beat;

	public void OnBeat() {
		if (Beat!=null) {
			Beat(this);
		}
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
			OnBeat();
		}
	}


}
