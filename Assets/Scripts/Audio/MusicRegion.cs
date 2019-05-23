using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRegion : MonoBehaviour {

	[SerializeField]
	private MusicPool pool;
	[SerializeField]
	private LayerMask layers;

	private void Start() {
		if (pool == null) {
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer))
			MusicManager.Instance.PushMusicPool(pool);
	}

	void OnTriggerExit(Collider other) {
		if (0 != (layers.value & 1 << other.gameObject.layer))
			MusicManager.Instance.PopMusicPool();
	}
}
