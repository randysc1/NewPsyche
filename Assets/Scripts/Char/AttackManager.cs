using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {


    public GameObject curBullModel;
    public GameObject meleeBox;
    public GameObject AOEPrefab;
    public GameObject MinePrefab;
    public GameObject FlashPrefab;
    public GameObject TrapPrefab;
    public GameObject MolotovPrefab;
    public GameObject WraithShroudPrefab;
    public GameObject tentaclePrefab;
    public GameObject tentaclePrefab2;

    //After tweaking, set to private
    public float BulletDamage;
    public bool shotgunEquipped = false;
    public int numPellets = 5;
    public int CurShotSpeed;
    public int shotgunSpeed;
    public float throwPower;
    public float GrabMinimumDistance;
    public float shotgunDelay;
    public float rangedDelay;
    public float sniperDelay;
    public float mineDelay;
    public float flashDelay;
    public float trapDelay;
    public float molotovDelay;
    private float tentacleDelay;
    private float attackCD;
    private float abilityCD;



    private GameObject AOEEffect;
    private GameObject tempShot;

    Animator anim;


    //When the melee animation changes, change this so the box only spawns for this long.
    private float meleeAnimDuration = .633f;
    private bool meleeing = false;

    private PhaseManager PM;
    private GameObject curMeleeBox;




    // Use this for initialization
    void Start()
    {
        PM = transform.gameObject.GetComponent<PhaseManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        if (abilityCD > 0 || attackCD > 0)
        {
            abilityCD -= Time.deltaTime;
            attackCD -= Time.deltaTime;
        }

        if (gameObject.GetComponent<PhaseManager>().Stunned)
        {
            return;
        }

        //Attack button Left Mouse
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (attackCD > 0)
            {
                return;
            }

            switch (PM.phase)
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

        //Ability button Right Mouse
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (abilityCD > 0)
            {
                return;
            }

            switch (PM.phase)
            {
                case 1:
                    sniper();
                    break;
                case 2:
                    molotov();
                    break;
                case 3:
                    wraithShroud();
                    break;
                default:
                    print("Phase is invalid,");
                    break;
            }
        }

        //Test area for other attacks.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shotgun();
        }

        if (abilityCD > 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            tentacleGrabShot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mine();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            flash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            molotov();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            trap();
        }
    }

    private void flash()
    {
        abilityCD = flashDelay;
        tempShot = Instantiate(FlashPrefab, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.transform.Rotate(-45, 0, 0);
        Rigidbody RB = tempShot.GetComponent<Rigidbody>();
        RB.AddForce(transform.forward * throwPower + transform.up * throwPower, ForceMode.Impulse);
        RB.AddTorque(transform.right * 10f, ForceMode.Impulse);

        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void molotov()
    {
        print("Molo");
        abilityCD = molotovDelay;
        tempShot = Instantiate(MolotovPrefab, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.transform.Rotate(-45, 0, 0);
        Rigidbody RB = tempShot.GetComponent<Rigidbody>();
        RB.AddForce(transform.forward * throwPower + transform.up * throwPower, ForceMode.Impulse);
        RB.AddTorque(transform.right * 10f, ForceMode.Impulse);

        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void mine()
    {
        abilityCD = mineDelay;
        tempShot = Instantiate(MinePrefab, this.transform.position + new Vector3(0, .5f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void trap()
    {
        abilityCD = trapDelay;
        tempShot = Instantiate(TrapPrefab, this.transform.position + new Vector3(0,.5f,0), transform.rotation, null);
        tempShot.SetActive(true);
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
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

    private void wraithShroud()
    {
        print("Shroud");
        AOEEffect = Instantiate(WraithShroudPrefab, this.transform.position, transform.rotation, this.transform);
        Destroy(AOEEffect, 2f);
    }

    //grabbed from controller.cs, changed so instantiate starts at player pos with player rotation.
    //Ranged Attack, spawns bullet firing away from player, bullet collider should proc damage on enemies.
    private void rangedAttack()
    {
        attackCD = rangedDelay;
        tempShot = Instantiate(curBullModel, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * CurShotSpeed;
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }

    //Spawns one bullet facing towards parent's forward, sets velocity to curShotSpeed
    private void sniper()
    {
        attackCD = sniperDelay;
        tempShot = Instantiate(curBullModel, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
        tempShot.SetActive(true);
        tempShot.GetComponent<Weapon>().shouldDissapate = false;
        tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * CurShotSpeed * 3;
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }


    //Spawn numPellets bullets, each firing forward after being turned randrange 10 degrees left or right.
    private void shotgun()
    {
        abilityCD = shotgunDelay;
        for (int i = 0; i <= numPellets; i++)
        {
            tempShot = Instantiate(curBullModel, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, null);
            tempShot.SetActive(true);
            Weapon Weap = tempShot.GetComponent<Weapon>();
            Weap.damage = BulletDamage / 2;
            tempShot.transform.Rotate(0, UnityEngine.Random.Range(-10, 10), 0);

            tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * shotgunSpeed;
            Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
        }
        return;
    }


    //Also grabbed from controller.cs.
    //Melee Attack, currently spawns 'sword' box in front of player, 
    private void meleeAttack()
    {
        attackCD = meleeAnimDuration;
        anim.SetTrigger("Attack");
        meleeBox.SetActive(true);
        Physics.IgnoreCollision(meleeBox.GetComponent<Collider>(), GetComponent<Collider>());
        StartCoroutine(meleeBoxDeactivation());
    }

    IEnumerator meleeBoxDeactivation()
    {
        yield return new WaitForSecondsRealtime(meleeAnimDuration);
        meleeBox.SetActive(false);        
    }


    private void tentacleGrab()
    {
        attackCD = tentacleDelay;
        tempShot = Instantiate(tentaclePrefab, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, this.gameObject.transform);
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void tentacleGrabShot()
    {
        attackCD = tentacleDelay;
        tempShot = Instantiate(tentaclePrefab2, this.transform.position + (transform.forward / 2) + new Vector3(0, .8f, 0), transform.rotation, this.gameObject.transform);
        Physics.IgnoreCollision(tempShot.GetComponent<Collider>(), GetComponent<Collider>());
        tempShot.GetComponent<Rigidbody>().velocity = tempShot.transform.forward * CurShotSpeed;

    }

    public void HandleGrab(GameObject Grabbed)
    {
        this.GetComponent<ThirdPersonCharacter>().MovementLocked = true;
        this.GetComponent<ThirdPersonCharacter>().RotationLocked = true;
        //If Grabbed has no rigidbody we don't care about it, so return
        if(Grabbed.GetComponent<Rigidbody>() == null)
        {
            return;
        }

        StartCoroutine(pullGrabbed(Grabbed));
    }

    IEnumerator pullGrabbed(GameObject Grabbed)
    {
        while(Vector3.Distance(Grabbed.transform.position, this.transform.position) > GrabMinimumDistance)
        {
            Vector3 GrabbedToUs = this.transform.position - Grabbed.transform.position;

            GrabbedToUs.Normalize();
            yield return new WaitForSecondsRealtime(.5f);

            Grabbed.GetComponent<Rigidbody>().AddForce(GrabbedToUs, ForceMode.Impulse);
        }
    }

    //This will eventually hold a switch case that changes details about the shot we fire, such as the prefab and speed
    public void ChangeBulletType(string newType)
    {
        print("Unimplemented change bullet type in AttackManager.cs");
    }

    public void HandlePowerUp(string powerupType, int duration)
    {
        switch (powerupType)
        {
            case "Shotgun":
                shotgunEquipped = true;
                //If we want a permanent change, then duration should be -1
                if(duration != -1)
                {
                    StartCoroutine(powerupDuration(powerupType, duration));
                }
                break;

            default:

                break;
        }
    }

    IEnumerator powerupDuration(string powerupType, int duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        stopPowerUp(powerupType);
    }

    private void stopPowerUp(string powerupType)
    {
        switch (powerupType)
        {
            case "Shotgun":
                shotgunEquipped = false;
                break;

            default:

                break;
        }
    }

}
