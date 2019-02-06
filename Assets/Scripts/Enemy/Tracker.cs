using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {

    private GameObject Player;
    public int sightDistance = 5;
    public int MoveSpeed = 4;
    public float MinDist = 0;
    public GameObject meleeBox;
    public GameObject tempBox;
    public bool Dead = false;

    private Transform playerTrans;
    private RaycastHit hit;

    // Use this for initialization
    void Start () {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        Player = Players[0];
	}

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            //If we die after swinging, disable the swing.
            if(tempBox != null)
            {
                tempBox.SetActive(false);
            }
            return;
        }
        //Putting this in update may eventually be costly, we might want to use active/deactivate zones in the future.

        playerTrans = Player.transform;
        CapsuleCollider Col = Player.GetComponent<CapsuleCollider>();
        transform.LookAt(playerTrans.position + new Vector3(0,1,0));

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, sightDistance))
        {

            if (hit.collider.gameObject.tag != "Player")
            {
                return;
            }

            if (Vector3.Distance(transform.position, playerTrans.position) >= MinDist)
            {

                transform.position += transform.forward * MoveSpeed * Time.deltaTime;

            } else
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        tempBox = Instantiate(meleeBox, transform.position + (transform.forward), transform.rotation, this.transform);
        tempBox.SetActive(true);
        Physics.IgnoreCollision(tempBox.GetComponent<Collider>(), GetComponent<Collider>());
        Destroy(tempBox, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
