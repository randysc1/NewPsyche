using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float BombRadius;
    public float Damage;
    public bool hitsPlayer = true;
    public bool hitsEnemy = true;
    public bool Flash;
    public bool DetonateOnGround;
    public float FlashDuration;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //On collision with trigger, generate sphere centered on this and register damage with collided
    private void OnTriggerEnter(Collider other)
    {
        if (!DetonateOnGround)
        {
            if(other.gameObject.tag == "Ground")
            {
                return;
            }
        }

        Collider[] hits = Physics.OverlapSphere(this.transform.position, BombRadius);
        Collider hit;

        for (int i = 0; i < hits.Length; i++)
        {
            hit = hits[i];
            if (hit.gameObject.tag == "Player")
            {
                PhaseManager PM = hit.gameObject.GetComponent<PhaseManager>();
                PM.TakeDamage(Damage);

                if (Flash)
                {
                    PM.SetStunned(FlashDuration);
                }
            } else if (hit.gameObject.tag == "Enemy")
            {
                EnemyHealth EH = hit.gameObject.GetComponent<EnemyHealth>();
                EH.TakeDamage(Damage);
                if (Flash)
                {
                    EH.SetStunned(FlashDuration);
                }
            }
        }
        ParticleSystem PS = GetComponent<ParticleSystem>();
        PS.Play();
        Destroy(this.gameObject, PS.main.duration);
    }
}
