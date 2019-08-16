using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRegion : MonoBehaviour {

	[SerializeField]
	private MusicPool pool;
	[SerializeField]
	private LayerMask layers;

	private bool inPool = false;

	void Start() {
		if (pool == null) {
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer)) {
			MusicManager.Instance.PushMusicPool(pool);
			inPool = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer)) {
			MusicManager.Instance.PopMusicPool();
			inPool = false;
		}
	}

	void OnDestroy() {
		if(inPool) {
			MusicManager.Instance.PopMusicPool();
		}
	}
}
