using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour {

    public float activationDelay;
    public float spawnRate;
    public float scaleRate;
    public GameObject TrackerPrefab;
    public GameObject FTrackerPrefab;
    public GameObject RangerPrefab;
    public GameObject FRangerPrefab;
    public GameObject TRangerPrefab;
    public GameObject JustSpawned;
    public string NameToActivate;
    private SpawnScaler Scaler;

    // Use this for initialization
    void Start()
    {
        Scaler = GameObject.Find("SpawnerScaler").GetComponent<SpawnScaler>();
        StartCoroutine(countdown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator countdown()
    {
        while (Scaler.ActivationDelay > 0)
        {
            Scaler.ActivationDelay -= Time.deltaTime;
            if (Scaler.ActivationDelay < 0)
            {
                switch (NameToActivate)
                {
                    case "TrackerPrefab":
                        SpawnTracker(spawnRate);
                        break;

                    case "FTrackerPrefab":
                        SpawnFTracker(spawnRate);
                        break;

                    case "RangerPrefab":
                        SpawnRanger(spawnRate);
                        break;
                    case "FRangerPrefab":
                        SpawnFRanger(spawnRate);
                        break;
                    case "TRangerPrefab":
                        SpawnTRanger(spawnRate);
                        break;

                    default:
                        print("Invalid input for spawner, make sure NameToActivate matches one of the gameobject var names above");
                        break;
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void Deactivate()
    {
        StopAllCoroutines();
    }

    public void Activate()
    {
        StartCoroutine(countdown());
    }

    public void Activate(string newEnemy)
    {
        NameToActivate = newEnemy;
        StartCoroutine(countdown());
    }

    public void Activate(string newEnemy, float newCD)
    {
        NameToActivate = newEnemy;
        activationDelay = newCD;
        StartCoroutine(countdown());

    }

    public void Activate(string newEnemy, float newCD, int countSpawned)
    {
        NameToActivate = newEnemy;
        activationDelay = newCD;
        StartCoroutine(countdown());
    }


    //base spawn enemy.
    private void SpawnEnemy(GameObject enemyType)
    {
        if (JustSpawned != null && JustSpawned.transform.position == transform.position + (transform.forward) + new Vector3(0, .8f, 0))
        {
        }
        else
        {
            JustSpawned = Instantiate(enemyType, transform.position + (transform.forward) + new Vector3(0, .8f, 0), transform.rotation, this.transform);
            JustSpawned.GetComponent<EnemyHealth>().SetStartingHealth((Time.time) * Scaler.ScalingRate);
            JustSpawned.SetActive(true);
        }
    }

    IEnumerator SpawnEnemy(GameObject enemyType, float spawnDelay)
    {
        while (true)
        {
            if (JustSpawned != null && JustSpawned.transform.position == transform.position + (transform.forward) + new Vector3(0, .8f, 0))
            {
            }
            else
            {
                JustSpawned = Instantiate(enemyType, transform.position + (transform.forward) + new Vector3(0, .8f, 0), transform.rotation, this.transform);
                JustSpawned.GetComponent<EnemyHealth>().SetStartingHealth((Time.time) * Scaler.ScalingRate);
                JustSpawned.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(Scaler.SpawnRate);
        }
    }

    IEnumerator SpawnEnemy(GameObject enemyType, int countSpawned)
    {
        while (countSpawned-->0)
        {
            if (JustSpawned != null && JustSpawned.transform.position == transform.position + (transform.forward) + new Vector3(0, .8f, 0))
            {
            }
            else
            {
                JustSpawned = Instantiate(enemyType, transform.position + (transform.forward) + new Vector3(0, .8f, 0), transform.rotation, this.transform);
                JustSpawned.GetComponent<EnemyHealth>().SetStartingHealth((Time.time) * Scaler.ScalingRate);
                JustSpawned.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(Scaler.SpawnRate);
        }
    }


    /*
     * Spawns tracker enemy.
     * 
     * */
    public void SpawnTracker()
    {
        SpawnEnemy(TrackerPrefab);
    }

    /*
     * Spawns tracker enemy every 'input' seconds.
     * */
    public void SpawnTracker(float spawnDelay)
    {
        StartCoroutine(SpawnEnemy(TrackerPrefab, Scaler.HowManyToSpawn));
    }


    /*
 * Spawns fast tracker enemy.
 * 
 * */
    public void SpawnFTracker()
    {
        SpawnEnemy(FTrackerPrefab);
    }

    /*
     * Spawns fast tracker enemy every 'input' seconds.
     * */
    public void SpawnFTracker(float spawnDelay)
    {
        StartCoroutine(SpawnEnemy(FTrackerPrefab, Scaler.HowManyToSpawn));
    }


    /*
* Spawns ranger enemy.
* 
* */
    public void SpawnRanger()
    {
        SpawnEnemy(RangerPrefab);
    }

    /*
     * Spawns ranger enemy every 'input' seconds.
     * */
    public void SpawnRanger(float spawnDelay)
    {
        StartCoroutine(SpawnEnemy(RangerPrefab, Scaler.HowManyToSpawn));
    }


    /*
* Spawns fast ranger enemy.
* 
* */
    public void SpawnFRanger()
    {
        SpawnEnemy(FRangerPrefab);
    }

    /*
     * Spawns fast ranger enemy every 'input' seconds.
     * */
    public void SpawnFRanger(float spawnDelay)
    {
        StartCoroutine(SpawnEnemy(FRangerPrefab, Scaler.HowManyToSpawn));
    }


    /*
* Spawns teleporting ranger enemy.
* 
* */
    public void SpawnTRanger()
    {
        SpawnEnemy(TRangerPrefab);
    }

    /*
     * Spawns teleporting ranger enemy every 'input' seconds.
     * */
    public void SpawnTRanger(float spawnDelay)
    {
        StartCoroutine(SpawnEnemy(TRangerPrefab, Scaler.HowManyToSpawn));
    }
}
