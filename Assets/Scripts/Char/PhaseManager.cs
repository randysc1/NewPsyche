using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{

    //Eventually want to turn to private
    public int phase;
    public GameObject bullet;
    public GameObject tempShot;
    public GameObject AOEEffect;

    public int ShotSpeed;
    public GameObject meleeBox;
    public GameObject tempBox;
    public GameObject AOEPrefab;


    // Use this for initialization
    void Start()
    {
        //We should try to find a prev phase if we have different level loading, otherwise set to full i guess?
        phase = 1;
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

        //Attack button
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

    private void AOEAttack()
    {
        print("Aoe");
        AOEEffect = Instantiate(AOEPrefab, this.transform.position, transform.rotation, this.transform);
        AOEEffect.SetActive(true);
        Physics.IgnoreCollision(AOEEffect.GetComponent<Collider>(), GetComponent<Collider>());

        Destroy(tempShot, .5f);
    }

    //grabbed from controller.cs, changed so instantiate starts at player pos with player rotation.
    //TODO: Figure out how to pack bullets into a child folder, looks messy otherwise.
    private void rangedAttack()
    {
        print("Ranged");
        tempShot = Instantiate(bullet, this.transform.position + (transform.forward / 2), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * ShotSpeed;
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());

        Destroy(tempShot, 5);
    }


    //Also grabbed from controller.cs.
    private void meleeAttack()
    {
        print("melee");
        tempBox = Instantiate(meleeBox, transform.position + (transform.forward), transform.rotation, this.transform);
        tempBox.SetActive(true);
        Physics.IgnoreCollision(tempBox.GetComponent<Collider>(), GetComponent<Collider>());
        Destroy(tempBox, 1);
    }
}
