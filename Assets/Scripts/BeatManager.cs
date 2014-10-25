using UnityEngine;
using System.Collections;

public class BeatManager : MonoBehaviour {
	public float gravity = -9.81f;

	public AudioClip beatSound;
	public AudioClip levelMusic;
	public int beatNumber = -4;
	public float beatLength = .5f;
	public int totalBeats = 100;
	float currentTime = 0.0f;
	float lastBeat = 0.0f;
	float startingTime = 0.0f;


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
		Physics.gravity = new Vector3(0, gravity, 0);
	}

	// Use this for initialization
	void Start () {
		//StartCoroutine("BeatUpdate");
	}
	public delegate void BeatEventHandler(BeatManager beatManager);
	public event BeatEventHandler Beat;

	public void OnBeat() {
		if (Beat!=null) {
			AudioSource.PlayClipAtPoint(beatSound,Camera.main.transform.position);
			//Debug.LogError(beatNumber);
			Beat(this);
		}
	}
	// Update is called once per frame
	/*void Update () {
		if (Input.GetKeyDown("space")){
			//OnBeat();
		}
	}*/
	IEnumerator BeatUpdate() {
		while(true) {
			yield return new WaitForSeconds(.5f);
			if (beatNumber == 0) {
				AudioSource.PlayClipAtPoint(levelMusic,Camera.main.transform.position);
			}
			//OnBeat();
			beatNumber++;
		}
	}
	void Update () {
		float passedTime = Time.time - lastBeat;
		if (passedTime >= beatLength) {
			if (beatNumber > 0) {
				lastBeat = startingTime + (float)beatNumber * beatLength;
			}
			if (beatNumber == 0) {
				AudioSource.PlayClipAtPoint(levelMusic,Camera.main.transform.position);
				lastBeat = Time.time;
				startingTime = Time.time;
			}
			OnBeat();
			if (beatNumber < 0) {
				lastBeat = Time.time;
			}
			beatNumber++;
		}

	}


}
