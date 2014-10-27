using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	
	public List<PuzzleUnit> puzzleUnits;
	private int _currentPuzzleUnit = 0;

	private float _currentStartTime = 0.0f;
	private float _currentEndTime = 0.0f;
	private AudioSource _audioSource;
	
	private static LevelManager instance;

	public bool readyToSwitchUnits = false;
	
	public Platform ground;
	
	public static LevelManager Instance
	{
		get 
		{
			return instance;
		}
	}
	void Awake() {
		instance = this;
		_audioSource = gameObject.GetComponent<AudioSource>() as AudioSource;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	
	public float getNextBeatLength (int beat) {
		PuzzleUnit curr = puzzleUnits[_currentPuzzleUnit];
		if (curr.BeatIsInRange(beat)) {
			//Debug.LogError(curr.beatPattern[beat]);
			return curr.getNextBeatLength(beat);
		} else { 
			if(_currentPuzzleUnit < puzzleUnits.Count) {
				/*if (MainCharacter.Instance.GetCurrentCell().transitionCell){
					SwitchUnits(beat);
					return puzzleUnits[_currentPuzzleUnit].getNextBeatLength(beat);
				} else {*/
					puzzleUnits[_currentPuzzleUnit].startingBeatNumber = beat;
					BeatManager.Instance.LoopBeatTo(curr.startingBeatNumber);
					playUnitClip(_currentPuzzleUnit);
					return curr.getNextBeatLength(beat);
				//}
			} else {
				Debug.LogError("You won I think!");
				return -1.0f;
			}
		}
	}

	public void SwitchUnits (int beat) {
		readyToSwitchUnits = false;
		MainCharacter.Instance.GetCurrentCell().transitionCell = false; //TODO make it smarter!
		_currentPuzzleUnit += 1;
		puzzleUnits[_currentPuzzleUnit].startingBeatNumber = beat;
		playUnitClip(_currentPuzzleUnit);
		iTween.MoveTo(Camera.main.gameObject,iTween.Hash("x",MainCharacter.Instance.gameObject.transform.position.x,"time",1.0));
	}



	public void playUnitClip (int unit) {
		_audioSource.Stop();
		_audioSource.clip = puzzleUnits[unit].unitMusic;
		_audioSource.Play();
		//Debug.LogError(unit);
	}
}
