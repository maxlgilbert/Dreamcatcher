using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
	Win,
	Lose,
	Pause
}
public class LevelManager : MonoBehaviour {
	
	public List<PuzzleUnit> puzzleUnits;
	//public int temporaryTotalPuzzleUnits = 4; // TODO DELETE THANKS
	private int _currentPuzzleUnit = 0;

	private float _currentStartTime = 0.0f;
	private float _currentEndTime = 0.0f;
	private AudioSource _audioSource;
	
	private static LevelManager instance;

	public bool readyToSwitchUnits = false;
	
	public Platform ground;
	public MovingPlatform movingGround;
	public MovingPlatform movingObstacle;
	public GameObject obstacle;

	private CellObject _currentTransitionCell;
	
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
		_currentTransitionCell = puzzleUnits [_currentPuzzleUnit].transitionCell;
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

	
	public delegate void GameStateHandler(GameState gameState);
	public event GameStateHandler GameStateChange;
	public void OnGameStateChange(GameState gameState) {
		if (GameStateChange!=null) {
			GameStateChange(gameState);
		}
	}


	public void SwitchUnits (int beat) {
		readyToSwitchUnits = false;
		MainCharacter.Instance.GetCurrentCell().transitionCell = false; //TODO make it smarter!
		_currentPuzzleUnit += 1;
		if (_currentPuzzleUnit < puzzleUnits.Count-1) {
			puzzleUnits[_currentPuzzleUnit].startingBeatNumber = beat;
			playUnitClip(_currentPuzzleUnit);
			_currentTransitionCell = puzzleUnits [_currentPuzzleUnit].transitionCell;
			Vector3 newCameraLocation = new Vector3 (_currentTransitionCell.gameObject.transform.position.x-12,MainCharacter.Instance.rigidbody.position.y, 0);
			iTween.MoveTo(Camera.main.gameObject,iTween.Hash("x",newCameraLocation.x,"y",newCameraLocation.y,"time",1.0));
		} else {
			OnGameStateChange(GameState.Win);
		}
	}



	public void playUnitClip (int unit) {
		_audioSource.Stop();
		_audioSource.clip = puzzleUnits[unit].unitMusic;
		_audioSource.Play();
		//Debug.LogError(unit);
	}

	public CellObject GetNextTransitionCell (int nextIndex) {
		if (nextIndex < puzzleUnits.Count) {
			return puzzleUnits[nextIndex].transitionCell;
		}
		return null;
	}

	public CellObject GetCheckPointCell() {
		return puzzleUnits[_currentPuzzleUnit].checkPointCell;
	}
}
