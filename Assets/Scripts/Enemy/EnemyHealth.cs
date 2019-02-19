using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float StartHealth;
    public float CurHealth;
    //Use this for invuln window later
    //private bool isDamaged = false;
    private int timer;
    public bool Dead = false;
    public bool Stunned;
    private int colorChanged = 0;
    private Color storageColor;

	// Use this for initialization
	void Start () {
        CurHealth = StartHealth;
        storageColor = GetComponentInChildren<MeshRenderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void ExplosionHit(float damage)
    {

    }

    public void SetStunned(float duration)
    {
        Stunned = true;
        StartCoroutine(RemoveStunned(duration));
    }

    IEnumerator RemoveStunned(float duration)
    {
        MeshRenderer myMesh = GetComponentInChildren<MeshRenderer>();
        myMesh.material.color = Color.yellow;
        colorChanged++;

        yield return new WaitForSecondsRealtime(duration);
        Stunned = false;
        colorChanged--;

        if (colorChanged == 0)
        {
            myMesh.material.color = storageColor;
        }
    }

    public void TakeDamage(float damage)
    {
        CurHealth -= damage;
        StartCoroutine(DamageColor());
        
        if(CurHealth <= 0)
        {
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
