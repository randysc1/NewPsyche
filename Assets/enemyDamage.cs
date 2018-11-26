using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDamage : MonoBehaviour {

	public int HP = 100;
	public bool hit = false;
	public float hitTime = 5f;
	public float timer;
	public Renderer enemyR;

	// Use this for initialization
	void Start () {
		timer = 0f;
		if(this.tag == "meleeEnemy" || this.tag == "rangedEnemy")
			enemyR = GetComponentInChildren<Renderer>();
		else
			enemyR = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

		if (hit == true) {
			timer += Time.deltaTime;
			if(timer >= hitTime){
				Debug.Log ("change color");
				enemyR.material.color = Color.grey;
				hit = false;
				timer = 0f;
			}
		}

	}

	void OnCollisionEnter(Collision col){
		//Debug.Log ("Collide");
		//Debug.Log (col);
		//Debug.Log (col.transform.tag);
		//Debug.Log (col.gameObject.tag);
		//Debug.Log (col.collider.tag);
		if (col.collider.tag == "Weapon") {
			HP = HP - 10;
			Debug.Log ("Melee Hit");
			//hitStart = Time.deltaTime
			hit = true;
			enemyR.material.color = Color.red;
		}
		if (col.collider.tag == "Bullet") {
			HP = HP - 10;
			//col.gameObject.SetActive(false);
			Destroy(col.gameObject);
			Debug.Log ("Ranged Hit");
			hit = true;
			enemyR.material.color = Color.red;
		}
		if (HP <= 0) {
			Destroy(this.gameObject);
		}
	}
}
