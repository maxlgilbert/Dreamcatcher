using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour {
	public float gravity = -9.81f;

	public AudioClip beatSound;
	//public List<AudioClip> levelMusic;
	//private int _currentLevelMusic = 0;
	public AudioListener listener;
	public int beatNumber = -4;
	//private AudioSource _audioSource;
	public float beatLength = .5f;
	private float _elapsedBeatTime = 0.0f;
	private float _timeToTwoSeconds = 100.0f;
	public float beatWindowPercent = .6f;
	private float beatWindow;
	public bool windowOpen;
	public int totalBeats = 100;
	float currentTime = 0.0f;
	float lastBeat = 0.0f;
	float startingTime = 0.0f;
	public bool running = true;
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
		beatWindow = beatWindowPercent*beatLength;
		windowOpen = false;
		//_audioSource = gameObject.GetComponent<AudioSource>() as AudioSource;
		Physics.gravity = new Vector3(0, gravity, 0);
	}

	// Use this for initialization
	void Start () {
		//StartCoroutine("BeatUpdate");
		LevelManager.Instance.GameStateChange += OnGameStateChanged;
	}
	public delegate void BeatEventHandler(BeatManager beatManager);
	public event BeatEventHandler Beat;
	public event BeatEventHandler BeatWindowChanged;

	public void OnBeat() {
		if (Beat!=null) {
			AudioSource.PlayClipAtPoint(beatSound,listener.transform.position);
			//Debug.LogError(beatNumber);
			Beat(this);
		}
	}

	public void OnBeatWindowChanged () {
		if (BeatWindowChanged != null) {
			//Debug.LogError(windowOpen);
			BeatWindowChanged(this);
		}
	}

	public void OnGameStateChanged (GameState gameState) {
		if (gameState == GameState.Win) {
			Debug.LogError("You won!");
			running = false;
		} else if (gameState == GameState.Lose) {
			Debug.LogError("You lose (add losing back in)!");
			//running = false;
		}
	}
	// Update is called once per frame
	/*void Update () {
		if (Input.GetKeyDown("space")){
			//OnBeat();
		}
	}*/
	/*IEnumerator BeatUpdate() {
		while(true) {
			yield return new WaitForSeconds(.5f);
			if (beatNumber == 0) {
				AudioSource.PlayClipAtPoint(levelMusic[_currentLevelMusic],listener.transform.position);
			}
			//OnBeat();
			beatNumber++;
		}
	}*/

	public void UpdateBeatInformation (float newBeatLength) {
		beatLength = newBeatLength;
		beatWindow = beatWindowPercent*beatLength;
	}

	public void LoopBeatTo (int newBeatNumber) {
		//beatNumber = newBeatNumber;
		/*_audioSource.Stop();
		_audioSource.clip = levelMusic[_currentLevelMusic];
		_audioSource.Play();*/

	}

	void Update () {
		if (running) {
			float passedTime = Time.time - lastBeat;

			if(Time.time - _timeToTwoSeconds >=1.98f) {
				if(LevelManager.Instance.readyToSwitchUnits) {
					LevelManager.Instance.SwitchUnits(beatNumber);
					//_elapsedBeatTime += beatLength;
					//lastBeat = startingTime + _elapsedBeatTime;
					_elapsedBeatTime = Time.time - startingTime;
					lastBeat = Time.time;
					//Debug.LogError(lastBeat);
					UpdateBeatInformation(LevelManager.Instance.getNextBeatLength(beatNumber));
					OnBeat();
					beatNumber++;
				}
				_timeToTwoSeconds = Time.time + (2.0f - (Time.time - _timeToTwoSeconds));
				//Debug.LogError("2!!");
			} else if (passedTime >= beatLength) {
				if (beatNumber > 0) {
					_elapsedBeatTime += beatLength;
					lastBeat = startingTime + _elapsedBeatTime;
					//Debug.LogError(lastBeat);
					UpdateBeatInformation(LevelManager.Instance.getNextBeatLength(beatNumber));
				}
				if (beatNumber == 0) {
					LevelManager.Instance.playUnitClip(0);
					lastBeat = Time.time;
					startingTime = Time.time;
					_timeToTwoSeconds = Time.time;
					UpdateBeatInformation(LevelManager.Instance.getNextBeatLength(beatNumber));
				}
				OnBeat();
				if (beatNumber < 0) {
					lastBeat = Time.time;
				}
				beatNumber++;
			}else if (beatNumber >= -1) {
				if (passedTime >= beatLength - beatWindow/2.0f) {
					if (!windowOpen){
						windowOpen = true;
						//Debug.LogError(beatNumber + " " + windowOpen + " " + passedTime);
						OnBeatWindowChanged();
					}
				}else if (passedTime >= beatWindow/2.0f && windowOpen) {
					windowOpen = false;
					//Debug.LogError(beatNumber + " " + windowOpen+ " " + passedTime);
					OnBeatWindowChanged();
				}
			}

		}
	}


}
