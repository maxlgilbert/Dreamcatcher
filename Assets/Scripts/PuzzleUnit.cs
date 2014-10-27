using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleUnit : MonoBehaviour {
	// Beat pattern
	[SerializeField] private List<float> beatPattern;

	// Completed?
	public bool completed = false;

	// Number of beats
	public int beatPatternLength;

	// Duration
	public float duration;

	// Checkpoints?
	public int startingBeatNumber=0;

	// Music
	public AudioClip unitMusic;
	
	// Location
	public int xIndex;
	public int yIndex;
	// Next unit?

	// Use this for initialization
	void Awake () {
		duration = 0.0f;
		beatPatternLength = beatPattern.Count;
		for (int i =0; i < beatPatternLength; i++) {
			duration+=beatPattern[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float getNextBeatLength (int beat) {
		int adjustedBeat = beat - startingBeatNumber;
		if (adjustedBeat < beatPattern.Count) {
			return beatPattern[adjustedBeat];
		}
		return -1.0f;
	}

	public bool BeatIsInRange (int beat) {
		//Debug.LogError (beat + " " + this.startingBeatNumber + " " + this.beatPatternLength);
		//Debug.LogError ((beat < this.startingBeatNumber+this.beatPatternLength));
		return (beat < this.startingBeatNumber+this.beatPatternLength);

	}
 
}
