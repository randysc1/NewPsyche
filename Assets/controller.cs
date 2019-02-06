using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class controller : MonoBehaviour {

	public Transform Hero;
	//public Transform SpawnPoint;
	//public Transform Phase1Hero;
	public Transform Phase2Hero;
	public Transform Phase3Hero;


	// Use this for initialization
	void Start () {
	
		//Hero = Instantiate (Phase1Hero, SpawnPoint.position, SpawnPoint.rotation);

	}
	
	// Update is called once per frame
	void Update () {

	
	}

	public void EnterPhase2(){
		Debug.Log ("phase2");
		Destroy (Hero.gameObject, 0);
		Hero = Instantiate (Phase2Hero, Hero.position, Hero.rotation);
		//Hero = Phase2Hero;
	}

	public void EnterPhase3(){
		Debug.Log ("phase3");
		Destroy (Hero.gameObject, 0);
		Hero = Instantiate (Phase3Hero, Hero.position, Hero.rotation);
		//Hero = Phase2Hero;
	}

}


	

