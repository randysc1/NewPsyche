using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{

    //Eventually want to turn to private
    public int phase;
    public int phase1Threshold;
    public int phase2Threshold;

    public GameObject curBullModel;
    public GameObject Sniperbullet;
    public GameObject meleeBox;
    public GameObject AOEPrefab;

    public int BulletDamage;
    public int ShotSpeed;

    public ParticleSystem PS;


    private GameObject HBar;
    private GameObject IBar;

    private float maxHealth = 100f;
    private float curHealth;
    private float maxIns = 100f;
    private float curIns;


    private GameObject AOEEffect;
    private GameObject tempBox;
    private GameObject tempShot;
    private Vector3 initialHBar;
    private int numHits;
    private Vector3 curRotate;
    

    private Animator m_Animator;
    //When the melee animation changes, change this so the box only spawns for this long.
    private float meleeAnimDuration = .633f;
    private bool meleeing = false;

    GameObject player;
    Animator anim;
    GameObject phase1;
    Animator p1Anim;
    RuntimeAnimatorController p1Controller;
    Avatar p1Avatar;
    GameObject phase2;
    Animator p2Anim;
    RuntimeAnimatorController p2Controller;
    Avatar p2Avatar;

    private GameObject curPhaseObj;





    // Use this for initialization
    void Start()
    {
        //Change this if we want to start with different ammo
        curBullModel = Sniperbullet;

        m_Animator = GetComponent<Animator>();
        //We should try to find a prev phase if we have different level loading, otherwise set to 1 I guess?
        phase = 1;
        

        anim = GetComponent<Animator>();
        player = GameObject.Find("/PlayerPrefab/Player");
        phase1 = GameObject.Find("/PlayerPrefab/Player/Ethan");
        p1Anim = phase1.GetComponent<Animator>();
        p1Controller = p1Anim.runtimeAnimatorController;
        p1Avatar = p1Anim.avatar;
        phase2 = GameObject.Find("/PlayerPrefab/Player/Wraith");
        p2Anim = phase2.GetComponent<Animator>();
        p2Controller = p2Anim.runtimeAnimatorController;
        p2Avatar = p2Anim.avatar;

        curPhaseObj = phase1;

        //Set starting full health.
        curHealth = 100f;
        curIns = 100f;


        HBar = GameObject.Find("HealthBar");
        IBar = GameObject.Find("InsanityBar");



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
            phase1.SetActive(true);
            phase2.SetActive(false);
            anim.runtimeAnimatorController = p1Controller;
            anim.avatar = p1Avatar;
            print("Changed to phase : " + phase);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            phase = 2;
            phase1.SetActive(false);
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            print("Changed to phase : " + phase);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            phase = 3;
            phase1.SetActive(false);
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
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
        AOEEffect = Instantiate(AOEPrefab, this.transform.position + new Vector3(0, .8f, 0), transform.rotation, this.transform);
        AOEEffect.SetActive(true);
        Physics.IgnoreCollision(AOEEffect.GetComponent<Collider>(), GetComponent<Collider>());

        Destroy(AOEEffect, .5f);
    }

    //grabbed from controller.cs, changed so instantiate starts at player pos with player rotation.
    //Ranged Attack, spawns bullet firing away from player, bullet collider should proc damage on enemies.
    private void rangedAttack()
    {
        print("Ranged");

        //Ray shot
        //curRotate = new Vector3(transform.forward.x, 0, transform.forward.z);
        //RaycastHit hit; 
        //Vector3 forward = transform.TransformDirection(curRotate) * 10;
        //Debug.DrawRay(transform.position + new Vector3(0, 1.5f, 0), forward, Color.red, 100, true);

        //if (Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), curRotate, out hit))
        //{
        //print("Hit : " + hit.transform.gameObject.tag);
        //if(hit.transform.gameObject.tag == "Enemy")
        //{
        //print("Hit");

        //hit.transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(BulletDamage);
        //}
        //}

        tempShot = Instantiate(curBullModel, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
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
        if (meleeing)
        {
            return;
        }
        meleeing = true;
        anim.SetTrigger("Attack");
        meleeBox.SetActive(true);
        Physics.IgnoreCollision(meleeBox.GetComponent<Collider>(), GetComponent<Collider>());
        StartCoroutine(meleeBoxDeactivation());
    }

    IEnumerator meleeBoxDeactivation()
    {
        yield return new WaitForSecondsRealtime(meleeAnimDuration);
        meleeBox.SetActive(false);
        meleeing = false;
    }

    public void TakeDamage(int howMuch)
    {
        //print("Oh heck, got hit!");
        //Figure out how to find the mesh renderer first before doing DamageColor
        //StartCoroutine(DamageColor());
        curHealth -= howMuch;
        curIns -= howMuch;

        if(curIns > phase1Threshold)
        {
            phase = 1;
            curPhaseObj.SetActive(false);
            phase1.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            curPhaseObj = phase1;
        } else if (curIns <= phase1Threshold && curIns > phase2Threshold)
        {
            curPhaseObj.SetActive(false);
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            phase = 2;
            curPhaseObj = phase2;
        } else if(curIns <= phase2Threshold)
        {
            curPhaseObj.SetActive(false);
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            phase = 3;
        }

        RefreshHealthAndIns();

    }

    private void RefreshHealthAndIns()
    {
        HBar.transform.localScale = new Vector3((curHealth / maxHealth), IBar.transform.localScale.y, 1);
        IBar.transform.localScale = new Vector3((curIns / maxIns), IBar.transform.localScale.y, 1);
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

    public void ChangeBulletType(string newType)
    {

    }
}
