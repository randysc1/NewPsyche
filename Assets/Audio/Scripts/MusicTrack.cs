using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack", menuName = "MusicTrackContainer")]
public class MusicTrack : ScriptableObject {
	public AudioClip musicStemLow,  musicStemHigh;

	public float bpm;
	public float endTime;
	public float[] outTimes;
	public float fadeTime = 0.25f;
}
