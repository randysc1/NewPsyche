using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public GameObject Egg;
	public GameObject Dragon;
	public GameObject GUI;
	private Animation anim;
	private bool started = false;
	private bool finished = false;
	private Component[] cracks;
	private Component[] GUItext;
	public ParticleSystem startParticle;
	public ParticleSystem eggExplodeParticle;
	public ParticleSystem glowParticle;
	public ParticleSystem backGlowParticle;
	public ParticleSystem dragonsParticle;
	public ParticleSystem hatchedParticle;


	// Use this for initialization
	void Start () {
		
		anim = Egg.GetComponent<Animation>();
		cracks = Egg.GetComponentsInChildren<SpriteRenderer>();
		GUItext = GUI.GetComponentsInChildren<SpriteRenderer> ();

		GUItext [0].gameObject.SetActive (false);

		startParticle.Play ();

	}
	
	// Update is called once per frame
	void Update () {

		if((Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Mouse0)) && started == false){

			started = true;

			//Turns off the 'tap to hatch' text
			GUItext [4].gameObject.SetActive (false);

			//Each animation plays one after the other
			anim.PlayQueued ("StretchSquash");
			anim.PlayQueued ("Shake");
			anim.PlayQueued ("SquashStretch");
			anim.PlayQueued ("Shake2");
			anim.PlayQueued ("SquashStretch(bigger)");

		}

		if (anim.IsPlaying ("SquashStretch")) {
			backGlowParticle.Play ();
			cracks[1].GetComponent<SpriteRenderer>().enabled = true;
		}

		if (anim.IsPlaying("Shake2")){
			glowParticle.Play();
			eggExplodeParticle.Play ();
		}

		if (anim.IsPlaying("SquashStretch(bigger)")){
			cracks [2].GetComponent<SpriteRenderer> ().enabled = true;
			finished = true;
		}

		if (!anim.isPlaying && finished == true) {

			backGlowParticle.Pause ();

			Egg.SetActive (false);
			hatchedParticle.Play ();

			Dragon.SetActive (true);
			dragonsParticle.Play ();

			//Make each bit of text visible
			GUItext [0].gameObject.SetActive (true);
			GUItext [1].GetComponent<SpriteRenderer> ().enabled = true;
			GUItext [2].GetComponent<SpriteRenderer> ().enabled = true;
			GUItext [3].GetComponent<SpriteRenderer> ().enabled = true;

		}
	}
}
