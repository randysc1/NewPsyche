using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public int StartHealth;
    public int CurHealth;
	// Use this for initialization
	void Start () {
        CurHealth = StartHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        CurHealth -= damage;
        StartCoroutine(DamageColor());
        //print("Ow! I got hit! Now at: " + CurHealth);
        
        if(CurHealth <= 0)
        {
            this.GetComponent<Tracker>().Dead = true;
            print("I died!");
            Destroy(this.gameObject,1);
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
}
