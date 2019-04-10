using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = GameObject.Find("Player").transform.position;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
