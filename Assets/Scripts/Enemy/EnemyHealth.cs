using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    public float StartHealth;
    public float CurHealth;
    //Use this for invuln window later
    //private bool isDamaged = false;
    private int timer;
    public bool Dead = false;
    public bool Stunned;
    public float StunDuration;
    private float onFire;
    private int colorChanged = 0;
    private Color storageColor;
    private NavMeshAgent NMA;


    // Use this for initialization
    void Start () {
        CurHealth = StartHealth;
        storageColor = GetComponentInChildren<MeshRenderer>().material.color;
        NMA = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(onFire > 0)
        {
            onFire -= Time.deltaTime;
            TakeDamage(1 * Time.deltaTime);
        }
	}

    public void SetStartingHealth(float newHealth){
        CurHealth = newHealth;
    }


    public void SetFire(float duration)
    {
        onFire = duration;
    }

    public void SetStunned(float duration)
    {
        Stunned = true;
        StunDuration = duration;
        NMA.destination = transform.position;
        StartCoroutine(RemoveStunned(duration));
    }

    IEnumerator RemoveStunned(float duration)
    {
        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.yellow;
        colorChanged++;

        yield return new WaitForSecondsRealtime(duration);
        Stunned = false;
        NMA.destination = GameObject.Find("Player").transform.position;
        StunDuration = 0;
        colorChanged--;

        if (colorChanged == 0)
        {
            myMesh.material.color = storageColor;
        }
    }

    public void TakeDamage(float damage)
    {
        if(damage == 0)
        {
            return;
        }
        CurHealth -= damage;
        StartCoroutine(DamageColor());
        
        if(CurHealth <= 0)
        {
            this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            Dead = true;
            Destroy(this.gameObject,1);
        }
    }

    //Wait a second, change back.
    private IEnumerator DamageColor()
    {
        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.red;
        colorChanged++;

        //Debug.Break();
        yield return new WaitForSecondsRealtime(1);

        colorChanged--;

        if(colorChanged == 0)
        {
            myMesh.material.color = storageColor;
        }
    }
}
