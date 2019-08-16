using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioUIManager : MonoBehaviour {

	[SerializeField]
	private AudioMixer masterMixer;

	[SerializeField]
	private AudioClip accept, decline, sliderTick;

	[SerializeField]
	private float volSFX = 0, volMusic = 0;
	[SerializeField]
	private bool muteSFX = false, muteMusic = false;

	public void SetVolSFX(float vol) {
		volSFX = CalculateCurve(vol);
		masterMixer.SetFloat("volSFX", volSFX);
	}

	public void SetVolMusic(float vol) {
		volMusic = CalculateCurve(vol);
		masterMixer.SetFloat("volMusic", volMusic);
	}

	public void toggleSFX() {
		if (!muteSFX) {
			masterMixer.SetFloat("volSFX", -80);
			muteSFX = true;
		} else {
			masterMixer.SetFloat("volSFX", volSFX);
			muteSFX = false;
		}
	}

	public void toggleMusic() {
		if (!muteMusic) {
			masterMixer.SetFloat("volMusic", -80);
			muteMusic = true;
		} else {
			masterMixer.SetFloat("volMusic", volMusic);
			muteMusic = false;
		}
	}

	public void PlayUIAccept() {
		AudioSource sound =  SoundManager.Instance.PlaySoundSFX(accept, this.gameObject);
		sound.spatialBlend = 0;
	}

	public void PlayUIDecline() {
		AudioSource sound = SoundManager.Instance.PlaySoundSFX(decline, this.gameObject);
		sound.spatialBlend = 0;
	}

	public void PlayUISliderTick() {
		AudioSource sound = SoundManager.Instance.PlaySoundSFX(sliderTick, this.gameObject);
		sound.spatialBlend = 0;
	}

	private float CalculateCurve(float valueIn) {
		return (Mathf.Pow(valueIn, 0.25f) * 80) - 80;
	}
}
