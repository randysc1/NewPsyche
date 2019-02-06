using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public int damage;
    //Type?
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print("Triggered!");
        if (other.transform.tag != "Player" && other.transform.tag != "Enemy")
        {
            print("Tag is: " + other.transform.tag);
            return;
        }

        print("Hit player or enemy!");
        if(other.transform.tag == "Player")
        {
            PhaseManager PM = other.gameObject.GetComponent<PhaseManager>();

            PM.TakeDamage(damage);
        } else
        {
            EnemyHealth Enemy = other.gameObject.GetComponent<EnemyHealth>();

            Enemy.TakeDamage(damage);
        }
    }
}
