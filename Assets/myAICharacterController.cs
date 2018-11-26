using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
	[RequireComponent(typeof (ThirdPersonCharacter))]
	public class myAICharacterController : MonoBehaviour
	{
		public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
		public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Transform target;                                    // target to aim for

		public GameObject meleeBox;
		public GameObject tempBox;
		public GameObject bullet;
		public GameObject tempShot;
		
		private void Start()
		{
			// get the components on the object we need ( should not be null due to require component so no need to check )
			agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();
			
			agent.updateRotation = false;
			agent.updatePosition = true;

			if (this.tag == "meleeEnemy") {
				//this.GetComponent
			}

			//if (target != null)
			//	agent.SetDestination (target.position);
		}
		
		
		private void Update()
		{
			if (target != null)
				agent.SetDestination (target.position);
			
			if (agent.remainingDistance > (agent.stoppingDistance+1)){

				if(this.tag == "rangedEnemy" && agent.remainingDistance <= 5){
					//if(agent.remainingDistance <= 5 && tempShot == null){
					character.Move (Vector3.zero, false, false);
					agent.isStopped = true;
					if(tempShot == null){
						//agent.updateRotation = true;
						agent.transform.LookAt(agent.pathEndPosition);
						tempShot = Instantiate (bullet, bullet.transform.position, bullet.transform.rotation, this.transform);
						tempShot.SetActive(true);
						//tempShot.transform = tempShot.transform.forward;
						tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward*4;
						tempShot.transform.parent = null;
						Destroy (tempShot, 3);
					}
				//	}
				}
				else{
					if(agent.isStopped == true)
						agent.isStopped = false;
					character.Move (agent.desiredVelocity, false, false);
					//Debug.Log ("Moving");
					//Debug.Log ("Remaining: " + agent.remainingDistance);
					//Debug.Log ("Stop: " + agent.stoppingDistance);
					//agent.SetDestination(target.position);
				}
			}
			else {
				character.Move (Vector3.zero, false, false);
				agent.isStopped = true;
				if(this.tag == "meleeEnemy" && tempBox == null){
					tempBox = Instantiate(meleeBox, meleeBox.transform.position, meleeBox.transform.rotation, this.transform);
					tempBox.SetActive(true);
					Destroy(tempBox, 1);
				}
				//Debug.Log ("Stopping");
			}
		}
		
		
		public void SetTarget(Transform target)
		{
			this.target = target;
		}
	}
}



//using System;
//using UnityEngine;


