using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {


    public string whichPower;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //print("Triggered!");
        if (other.transform.tag == "Player")
        {
           ///Do the attackManager change here
        }
    }
}
