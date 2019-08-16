using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAddPool : MonoBehaviour {

	[SerializeField]
	private MusicPool pool;
	[SerializeField]
	private bool delayStart = false;
	private bool started;

	void Start () {
		if (!delayStart) {
			MusicManager.Instance.PushMusicPool(pool);
			started = true;
		}
	}

	void LateUpdate() {
		if (!started) {
			MusicManager.Instance.PushMusicPool(pool);
			started = true;
		}
	}

	void OnDestroy() {
		MusicManager.Instance.PopMusicPool();
	}
}
