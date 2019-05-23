using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager Instance;

	[SerializeField]
	private GameObject musicAudioSourcePrefab;

	[SerializeField]
	private bool playMusicOnStart = true;
	private bool musicPlaying = false;
	[SerializeField]
	private bool cueNewPool = false;
	[SerializeField]
	private bool hardOut = false;

	[SerializeField]
	private float intensity = 0f;
	[SerializeField]
	private float nextEndTime = 0f;
	[SerializeField]
	private float nextOutTime = 0f;
	[SerializeField]
	private float nextBeat = 0f;


	[SerializeField]
	private MusicPool activePool;
	[SerializeField]
	private MusicTrack activeTrack;
	private AudioSource[] activeSources = new AudioSource[2];

	private Stack<MusicPool> musicStack = new Stack<MusicPool>();

	void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start () {
		if (playMusicOnStart && activePool) {
			StartMusic();
		}
	}
	
	void Update () {
		if (Time.time >= nextEndTime && musicPlaying) {
			float destroyTime = activeSources[0].clip.length - activeSources[0].time;
			Destroy(activeSources[0].gameObject, destroyTime);
			Destroy(activeSources[1].gameObject, destroyTime);
			PlayNewTrack();
		}
		if (Time.time >= nextOutTime && musicPlaying) {
			if (cueNewPool) {
				FadeCurrentTrack();
				PlayNewTrack();
			} else {
				SetNextOutTime();
			}
		}
		if (Time.time >= nextBeat && musicPlaying) {
			if (hardOut) {
				FadeCurrentTrack();
				PlayNewTrack();
				hardOut = false;
			} else {
				SetNextBeat();
			}
		}
	}

	public void StartMusic() {
		if (!musicPlaying) {
			PlayNewTrack();
			musicPlaying = true;
		}
	}

	public void PushMusicPool(MusicPool newPool) {
		musicStack.Push(newPool);
		activePool = musicStack.Peek();
		cueNewPool = true;
	}

	public void PopMusicPool() {
		musicStack.Pop();
		activePool = musicStack.Peek();
		cueNewPool = true;
	}

	public void TransitionMusicNow() {
		hardOut = true;
	}

	public void SetIntensity(float newLevel, float fadeTime) {
		StartCoroutine(FadeIntensity(newLevel, fadeTime));
	}

	private void PlayNewTrack() {
		activeTrack = ReturnNewTrack();

		AudioSource freshMusicSource = Instantiate(musicAudioSourcePrefab).GetComponent<AudioSource>();
		freshMusicSource.gameObject.transform.parent = gameObject.transform;
		freshMusicSource.clip = activeTrack.musicStemLow;
		freshMusicSource.volume = 1f - intensity;
		activeSources[0] = freshMusicSource;

		freshMusicSource = Instantiate(musicAudioSourcePrefab).GetComponent<AudioSource>();
		freshMusicSource.gameObject.transform.parent = gameObject.transform;
		freshMusicSource.clip = activeTrack.musicStemHigh;
		freshMusicSource.volume = intensity;
		activeSources[1] = freshMusicSource;

		SetNextEndTime();
		SetNextOutTime();
		SetNextBeat();

		activeSources[0].Play();
		activeSources[1].Play();

		cueNewPool = false;
	}

	private void FadeCurrentTrack() {
		StartCoroutine(FadeOutAndStop(activeSources[0], activeTrack.fadeTime));
		StartCoroutine(FadeOutAndStop(activeSources[1], activeTrack.fadeTime));
	}

	private MusicTrack ReturnNewTrack() {
		return activePool.musicStems[Random.Range(0,activePool.musicStems.Length)];
	}

	private void SetNextEndTime() {
		nextEndTime = activeTrack.endTime - activeSources[0].time + Time.time;
	}

	private void SetNextOutTime() {
		bool changed = false;
		for (int i = activeTrack.outTimes.Length - 1; i >= 0; i--) {
			if (activeTrack.outTimes[i] > activeSources[0].time) {
				nextOutTime = activeTrack.outTimes[i] - activeSources[0].time + Time.time;
				changed = true;
			}
		}
		if (!changed) {
			nextOutTime = activeSources[0].clip.length + Time.time;
		}
	}

	private void SetNextBeat() {
		nextBeat = activeSources[0].time % (60 / activeTrack.bpm) + Time.time + 1;
	}



	IEnumerator FadeOutAndStop(AudioSource source, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startVolume = source.volume;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			source.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
			yield return null;
		}

		source.Stop();
		Destroy(source.gameObject);

	}

	IEnumerator FadeOut(AudioSource source, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startVolume = source.volume;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			source.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeTime);
			yield return null;
		}

		source.volume = 0f;

	}

	IEnumerator FadeIn(AudioSource source, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startVolume = source.volume;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			source.volume = Mathf.Lerp(startVolume, 1f, currentTime / fadeTime);
			yield return null;
		}

		source.volume = 1f;

	}

	IEnumerator FadeTo(AudioSource source, float newVolume, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startVolume = source.volume;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			source.volume = Mathf.Lerp(startVolume, newVolume, currentTime / fadeTime);
			yield return null;
		}
	}

	IEnumerator FadeIntensity(float newLevel, float fadeTime) {
		float startTime = Time.time;
		float currentTime = 0f;
		float startLevel = intensity;

		while (startTime + fadeTime > Time.time) {
			currentTime = Time.time - startTime;

			intensity = Mathf.Lerp(startLevel, newLevel, currentTime / fadeTime);
			activeSources[0].volume = 1 - intensity;
			activeSources[1].volume = intensity;
			yield return null;
		}
	}
}
