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

    public int BulletDamage;
    public int ShotSpeed;

    public ParticleSystem PS;


    private GameObject HBar;
    private GameObject IBar;

    private int maxHealth = 100;
    private int curHealth;
    private int maxIns = 100;
    private int curIns;


    private GameObject AOEEffect;
    private GameObject tempBox;
    private GameObject tempShot;
    private Vector3 initialHBar;
    private int numHits;
    private Vector3 curRotate;

    private Animator m_Animator; 





    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        //We should try to find a prev phase if we have different level loading, otherwise set to 1 I guess?
        phase = 1;

        //Set starting full health.
        curHealth = 100;
        curIns = 100;

        
        HBar = GameObject.Find("HBarSprite");
        IBar = GameObject.Find("IBarSprite");


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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (phase)
            {
                case 1:
                    rangedAttack();
                    break;
                case 2:
                    meleeAttack();
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
    private void rangedAttack()
    {
        print("Ranged");
        curRotate = new Vector3(transform.forward.x, 0, transform.forward.z);
        RaycastHit hit; 
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), forward, Color.red, 100, true);

        if (Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), curRotate, out hit))
        {
            print("Hit : " + hit.transform.gameObject.tag);
            if(hit.transform.gameObject.tag == "Enemy")
            {
                print("Hit");

                hit.transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(BulletDamage);
            }
        }

        //Old shot
        //tempShot = Instantiate(bullet, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        //tempShot.SetActive(true);
        //tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * ShotSpeed;
        //tempShot.transform.rotation = new Quaternion(tempShot.transform.rotation.x, 0, tempShot.transform.rotation.z, tempShot.transform.rotation.w);
        //Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
        //Destroy(tempShot, 5);
    }

    //Also grabbed from controller.cs.
    //Melee Attack, currently spawns 'sword' box in front of player, 
    private void meleeAttack()
    {
        //print("melee");
        m_Animator.SetTrigger("Attack");
        meleeBox.SetActive(true);
        Physics.IgnoreCollision(meleeBox.GetComponent<Collider>(), GetComponent<Collider>());
        StartCoroutine(meleeBoxDeactivation());

        Destroy(tempBox, 1);
    }

    IEnumerator meleeBoxDeactivation()
    {
        yield return new WaitForSecondsRealtime(1);
        meleeBox.SetActive(false);
    }

    public void TakeDamage(int howMuch)
    {
        //print("Oh heck, got hit!");
        //Figure out how to find the mesh renderer first before doing DamageColor
        //StartCoroutine(DamageColor());
        curHealth -= howMuch;
        curIns -= howMuch;

        RefreshHealthAndIns();       
        
    }

    private void RefreshHealthAndIns()
    {
        HBar.transform.localScale = new Vector3((curHealth / maxHealth) * 40, 5, 1);
        IBar.transform.localScale = new Vector3((curIns / maxIns) * 40, 5, 1);
    }

    //Wait a second, change back.
    IEnumerator DamageColor()
    {
        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        Color storage = myMesh.material.color;
        myMesh.material.color = Color.red;
        yield return new WaitForSecondsRealtime(1);
        myMesh.material.color = storage;
    }
}
