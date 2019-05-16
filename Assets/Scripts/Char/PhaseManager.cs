using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{

    //Eventually want to turn to private
    public int phase;
    //When Under phase2Threshold we go to phase2
    public int phase2Threshold;
    //When Under phase3Threshold we go to phase 3
    public int phase3Threshold;

    private GameObject HBar;
    private GameObject IBar;
    public GameObject GOText;

    private float maxHealth = 100f;
    private float curHealth;
    private float maxIns = 100f;
    private float curIns;
    public float RegenDelay;
    public int RegenAmountPerSecond;
    private float curDelay;


    private GameObject AOEEffect;
    private GameObject tempBox;
    private GameObject tempShot;
    private Vector3 initialHBar;
    private int numHits;
    private Vector3 curRotate;
    private float restartTimer;
    private float restartDelay = 5f;
    public bool Stunned;

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

    private AttackManager AM;

    ThirdPersonCharacter TPC;



    // Use this for initialization
    void Start()
    {

        AM = transform.gameObject.GetComponent<AttackManager>();
        //We should try to find a prev phase if we have different level loading, otherwise set to 1 I guess?
        phase = 1;

        TPC = gameObject.GetComponent<ThirdPersonCharacter>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("/PlayerPrefab/Player");
        phase1 = GameObject.Find("/PlayerPrefab/Player/Female");
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
    }

    // Update is called once per frame
    void Update()
    {
        //Bit redundant with the take damage already checking, keep?
        if (curHealth <= 0)
        {
            die();
            return;
        }
        //If curDelay is greater than zero, decrement the time.
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;

            //If the curDelay is less than 0 and the health isn't full, regen health.
        } else if (curHealth < 100) 
        {
            //We relate the amount to the time.deltaTime so that if update works faster or slower we regen the same amount.
            curHealth += (RegenAmountPerSecond * Time.deltaTime);
            RefreshHealthAndIns();
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            if(phase == 1)
            {
                return;
            } else
            {
                phase--;
                print("phase-- is now :" + phase);

                ChangeToPhase(phase);
            }
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {

            if (phase == 3)
            {
                return;
            }
            else
            {
                phase++;
                print("phase++ is now :" + phase);

                ChangeToPhase(phase);
            }
        }

        return;
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

    }

    public void ExplosionHit(float damage)
    {

    }

    public void SetStunned(float duration)
    {
        print("Got stunned");
        Stunned = true;
        StartCoroutine(RemoveStunned(duration));
    }

    IEnumerator RemoveStunned(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Stunned = false;
    }

    public void removeIns(float howMuch)
    {
        if(curIns - howMuch < 0)
        {
            curIns = 0;
        } else
        {
            curIns = curIns - howMuch;
        }
        checkPhaseTransition();
        RefreshHealthAndIns();

    }


    public void TakeDamage(float howMuch)
    {
        //print("Oh heck, got hit!");
        //Figure out how to find the mesh renderer first before doing DamageColor
        //StartCoroutine(DamageColor());
        //Change health/Ins values
        if (howMuch > curHealth)
        {
            curHealth = 0;
        }
        else
        {
            curHealth -= howMuch;
        }
        if (howMuch > curIns)
        {
            curIns = 0;
        }
        else
        {
            curIns -= howMuch;
        }
        curDelay = RegenDelay;


        //Handle Death
        if (curHealth <= 0)
        {
            die();
        }

        checkPhaseTransition();

        RefreshHealthAndIns();

    }

    private void checkPhaseTransition()
    {
        //If your insanity is above threshold 2, should nothing happen?
        if (curIns > phase2Threshold)
        {

            //If damaged under the threshold, enter phase 2
        }
        else if (curIns <= phase2Threshold && curIns > phase3Threshold && phase != 2)
        {
            ChangeToPhase(2);
        }
        else if (curIns <= phase3Threshold && phase != 3)
        {
            ChangeToPhase(3);
        }
    }

    private void RefreshHealthAndIns()
    {
        HBar.transform.localScale = new Vector3((curHealth / maxHealth), IBar.transform.localScale.y, 1);
        IBar.transform.localScale = new Vector3((curIns / maxIns), IBar.transform.localScale.y, 1);
    }

    private void ChangeToPhase(int newPhase)
    {
        curPhaseObj.SetActive(false);
        if (newPhase == 1)
        {
            phase1.SetActive(true);
            anim.runtimeAnimatorController = p1Controller;
            anim.avatar = p1Avatar;
            phase = 1;
            curPhaseObj = phase1;
        }
        else if (newPhase == 2)
        {
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            phase = 2;
            curPhaseObj = phase2;
        }
        //Don't have phase 3 yet, so set to 2
        else if (newPhase == 3)
        {
            curPhaseObj.SetActive(false);
            phase2.SetActive(true);
            anim.runtimeAnimatorController = p2Controller;
            anim.avatar = p2Avatar;
            phase = 3;
        }
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


    //This will eventually hold a switch case that changes details about the shot we fire, such as the prefab and speed
    public void ChangeBulletType(string newType)
    {

    }


    private void die()
    {
        GOText.SetActive(true);

        TPC.MovementLocked = true;
        TPC.RotationLocked = true;
        AM.CanAttack = false;
        // .. increment a timer to count up to restarting.
        restartTimer += Time.deltaTime;

        // .. if it reaches the restart delay...
        if (restartTimer >= restartDelay)
        {
            // .. then reload the currently loaded level.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            return;
        }
    }
   
}
