using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawner : MonoBehaviour {

    //Delay between activate and actual spawning.
    public float activationDelay;
    //Time between spawns
    public float spawnRate = 5;
    public float scaleRate;
    private float countToSpawn;
    public GameObject TrackerPrefab;
    public GameObject FTrackerPrefab;
    public GameObject RangerPrefab;
    public GameObject FRangerPrefab;
    public GameObject TRangerPrefab;
    public GameObject JustSpawned;
    public string NameToActivate;
    public bool IsActive;
    private GameObject currentPrefab;
    private SpawnScaler Scaler;

    // Use this for initialization
    void Start()
    {
        Scaler = GameObject.Find("SpawnerScaler").GetComponent<SpawnScaler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator countdown()
    {
        while (activationDelay > 0)
        {
            activationDelay -= Time.deltaTime;
            if (activationDelay < 0)
            {
                switch (NameToActivate)
                {
                    case "TrackerPrefab":
                        currentPrefab = TrackerPrefab;
                        break;

                    case "FTrackerPrefab":
                        currentPrefab = FTrackerPrefab;
                        break;

                    case "RangerPrefab":
                        currentPrefab = RangerPrefab;
                        break;
                    case "FRangerPrefab":
                        currentPrefab = FRangerPrefab;
                        break;
                    case "TRangerPrefab":
                        currentPrefab = TRangerPrefab;
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
            SpawnEnemy(countToSpawn);
        }
    }

    public void Deactivate()
    { 
        StopAllCoroutines();
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;

        StartCoroutine(countdown());
    }

    public void Activate(string newEnemy)
    {
        IsActive = true;
        NameToActivate = newEnemy;
        StartCoroutine(countdown());
    }

    public void Activate(string newEnemy, float newCD)
    {
        IsActive = true;
        NameToActivate = newEnemy;
        activationDelay = newCD;
        StartCoroutine(countdown());

    }

    public void Activate(string newEnemy, float newCD, int countSpawned)
    {
        IsActive = true;
        NameToActivate = newEnemy;
        activationDelay = newCD;
        countToSpawn = countSpawned;
        StartCoroutine(countdown());
    }

    public void Activate(string newEnemy, int countSpawned)
    {
        IsActive = true;
        NameToActivate = newEnemy;
        countToSpawn = countSpawned;
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

    IEnumerator SpawnEnemy(float spawnDelay)
    {
        while (true)
        {
            if (JustSpawned != null && JustSpawned.transform.position == transform.position + (transform.forward) + new Vector3(0, .8f, 0))
            {}
            else
            {
                JustSpawned = Instantiate(currentPrefab, transform.position + (transform.forward) + new Vector3(0, .8f, 0), transform.rotation, this.transform);
                JustSpawned.GetComponent<EnemyHealth>().SetStartingHealth((Time.time) * Scaler.ScalingRate);
                JustSpawned.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(Scaler.SpawnRate);
        }
    }

    IEnumerator SpawnEnemy(int countSpawned)
    {
        while (countSpawned-->0)
        {
            if (JustSpawned != null && JustSpawned.transform.position == transform.position + (transform.forward) + new Vector3(0, .8f, 0))
            {
                countSpawned++;
            }
            else
            {
                JustSpawned = Instantiate(currentPrefab, transform.position + (transform.forward) + new Vector3(0, .8f, 0), transform.rotation, this.transform);
                JustSpawned.GetComponent<EnemyHealth>().SetStartingHealth((Time.time) * Scaler.ScalingRate);
                JustSpawned.SetActive(true);
            }

            yield return new WaitForSecondsRealtime(Scaler.SpawnRate);
        }
        IsActive = false;
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
 * Spawns fast tracker enemy.
 * 
 * */
    public void SpawnFTracker()
    {
        SpawnEnemy(FTrackerPrefab);
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
* Spawns fast ranger enemy.
* 
* */
    public void SpawnFRanger()
    {
        SpawnEnemy(FRangerPrefab);
    }

    /*
* Spawns teleporting ranger enemy.
* 
* */
    public void SpawnTRanger()
    {
        SpawnEnemy(TRangerPrefab);
    }

}
