using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {


    public string whichPower;
    public int duration;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddTorque(transform.up * 10f, ForceMode.Impulse);
        this.transform.Rotate(-45, 0, 0);

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //print("Triggered!");
        if (other.transform.tag == "Player")
        {
            other.GetComponent<AttackManager>().HandlePowerUp(whichPower, duration);
            Destroy(this.gameObject, 0);
        }
    }
}
