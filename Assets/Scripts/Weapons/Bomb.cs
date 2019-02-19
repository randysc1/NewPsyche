using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float BombRadius;
    public float Damage;
    public bool hitsPlayer = true;
    public bool hitsEnemy = true;
    public bool Flash;
    public bool Ignite;
    public bool DetonateOnGround;
    public float FlashDuration;
    public float IgniteDuration;

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
        //If we don't want to detonate on the ground and we just hit the ground ignore it
        if (!DetonateOnGround)
        {
            if(other.gameObject.tag == "Ground")
            {
                return;
            }
        }

        //Create a sphere for the 'explosion' and proc damage on all collisions of appropriate type
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
                if (Ignite)
                {
                   // PM.SetStunned(FlashDuration);
                }
            } else if (hit.gameObject.tag == "Enemy")
            {
                EnemyHealth EH = hit.gameObject.GetComponent<EnemyHealth>();
                EH.TakeDamage(Damage);

                if (Flash)
                {
                    EH.SetStunned(FlashDuration);
                }
                if (Ignite)
                {
                    EH.SetFire(IgniteDuration);
                }
            }
        }

        //ParticleSystem PS = Instantiate(GetComponent<ParticleSystem>(), transform.position, transform.rotation, null);
        ParticleSystem PS = GetComponent<ParticleSystem>();
        PS.Play();
        Destroy(this.gameObject, PS.main.duration);
    }
}
