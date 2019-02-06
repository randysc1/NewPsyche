using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{

    //Eventually want to turn to private
    public int phase;

    public GameObject bullet;
    public GameObject meleeBox;
    public GameObject AOEPrefab;

    public int ShotSpeed;
    public ParticleSystem PS;
    public GameObject HBar;


    private GameObject AOEEffect;
    private GameObject tempBox;
    private GameObject tempShot;
    private Vector3 initialHBar;
    private int numHits;




    // Use this for initialization
    void Start()
    {
        //We should try to find a prev phase if we have different level loading, otherwise set to 1 I guess?
        phase = 1;


        //initialHBar = new Vector3 (HBar.size ().x, HBar.size ().y);
        //initialHBar = new Vector3(HBar.GetComponent<RectTransform>().sizeDelta.x, HBar.GetComponent<RectTransform>().sizeDelta.y);
        numHits = 10;

        if (PS != null)
        {
            PS.Play();
            PS.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dev mode change phases
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            phase = 1;
            print("Changed to phase : " + phase);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            phase = 2;
            print("Changed to phase : " + phase);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            phase = 3;
            print("Changed to phase : " + phase);

        }

        //Attack button X
        if (Input.GetKeyDown(KeyCode.X))
        {
            switch (phase)
            {
                case 1:
                    meleeAttack();
                    break;
                case 2:
                    rangedAttack();
                    break;
                case 3:
                    AOEAttack();
                    break;
                default:
                    print("Phase is invalid,");
                    break;
                
            }
        }
    }

    //AOE attack, spawns aoe sphere at feet, sphere collider should proc damage on enemies.
    private void AOEAttack()
    {
        //print("Aoe");
        AOEEffect = Instantiate(AOEPrefab, this.transform.position + new Vector3(0,.8f,0), transform.rotation, this.transform);
        AOEEffect.SetActive(true);
        Physics.IgnoreCollision(AOEEffect.GetComponent<Collider>(), GetComponent<Collider>());

        Destroy(AOEEffect, .5f);
    }

    //grabbed from controller.cs, changed so instantiate starts at player pos with player rotation.
    //Ranged Attack, spawns bullet firing away from player, bullet collider should proc damage on enemies.
    //TODO: Figure out how to pack bullets into a child folder, looks messy otherwise.
    private void rangedAttack()
    {
        //print("Ranged");
        tempShot = Instantiate(bullet, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * ShotSpeed;
        tempShot.transform.rotation = new Quaternion(tempShot.transform.rotation.x, 0, tempShot.transform.rotation.z, tempShot.transform.rotation.w);
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());

        Destroy(tempShot, 5);
    }

    //Also grabbed from controller.cs.
    //Melee Attack, currently spawns 'sword' box in front of player, 
    private void meleeAttack()
    {
        //print("melee");
        tempBox = Instantiate(meleeBox, transform.position + (transform.forward) + new Vector3(0,.8f,0), transform.rotation, this.transform);
        tempBox.SetActive(true);
        Physics.IgnoreCollision(tempBox.GetComponent<Collider>(), GetComponent<Collider>());
        Destroy(tempBox, 1);
    }

    public void TakeDamage(int howMuch)
    {
        print("Oh heck, got hit!");

        
    }
}
