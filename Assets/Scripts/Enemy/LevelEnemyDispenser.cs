using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEnemyDispenser : MonoBehaviour {



    private GameObject player;
    private GameObject[] allSpawnersBase;
    private System.Random ran = new System.Random();


    // Use this for initialization
    void Start () {
        player = GameObject.Find("PlayerPrefab");
        allSpawnersBase = GameObject.FindGameObjectsWithTag("Spawner");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //If left is closer, return 0
    private int CompareDistance(GameObject left, GameObject right)
    {
        if (Vector3.Distance(player.transform.position, left.transform.position) < Vector3.Distance(player.transform.position, right.transform.position)){
            return 0;
        } else
        {
            return 1;
        }
    }
    /*
     * Scan for the furthest three spawners, and return the furthest three
     */
    private GameObject[] getFarSpawners()
    {
        //Make a shallow clone of pointers to gameobjects because of the 
        GameObject[] allSpawners = (GameObject[]) allSpawnersBase.Clone();

        //If the number of spawners found is less than 3, use all 3.
        if (allSpawners.Length < 3)
        {
            Debug.Log("Please Add more than 3 total Advanced spawners in the level");
            Debug.Break();
        } else if (allSpawners.Length == 3)
        {
            return allSpawners;
        }

        GameObject[] farthestSpawners = new GameObject[3];

        for (int i = 0; i < 3; i++)
        {
            farthestSpawners[i] = new GameObject();
        }

        IEnumerable<int> sequence = Enumerable.Range(0, allSpawners.Length - 1).OrderBy(n => n * n * (ran).Next());
        IEnumerable<int> result = sequence.Distinct().Take(3);
        var enumerator = result.GetEnumerator();

        int badName = 0;
        //Iterate through each of the random numbers
        while (enumerator.MoveNext())
        {
            badName = enumerator.Current;

            farthestSpawners[0] = allSpawners[badName];
            enumerator.MoveNext();
            badName = enumerator.Current;

            farthestSpawners[1] = allSpawners[badName];
            enumerator.MoveNext();
            badName = enumerator.Current;

            farthestSpawners[2] = allSpawners[badName];
            enumerator.MoveNext();
            return farthestSpawners;
        }

        //Compare all of the spawners vs the three saved farthest and replace if possible. 
        for (int i = 0; i < allSpawners.Length; i++)
        {
           
            //Loop for the farthest spawners we currently have.
            for(int j = 0;  j < farthestSpawners.Length; j++)
            {


                //If the current entry we're on is null, tested by checking if it has a 
                //TimedSpawner script. Then replace this entry with the spawner we're on.
                if(farthestSpawners[j].GetComponent<TimedSpawner>() == null)
                {
                    GameObject toDelete = farthestSpawners[j];
                    farthestSpawners[j] = allSpawners[i];
                    Destroy(toDelete);
                    break;

                    //If it is a valid entry, check if it's closer than the current spawner
                    //we're comparing with, 
                } else
                {
                    if (GameObject.ReferenceEquals(farthestSpawners[j], allSpawners[i]))
                    {
                        continue;
                    }

                    if (CompareDistance(farthestSpawners[j], allSpawners[i]) == 0)
                    {
                        //If the new one is further, save the old furthest spawner in a temp
                        //and iterate up the rest of the furthestSpawners array 
                        GameObject tempSpawner1 = farthestSpawners[j];                        
                        farthestSpawners[j] = allSpawners[i];

                        //Iterating starts at the entry index we're on + 1,
                        //(so if entry at index 2, we skip.), we then store a 
                        //temp, swap the appropriate one in, and reset the temp to new temp.
                        for(int n = j + 1; n < farthestSpawners.Length; n++)
                        {
                            GameObject tempSpawner2 = farthestSpawners[n];
                            farthestSpawners[n] = tempSpawner1;
                            tempSpawner1 = tempSpawner2;
                        }
                    }
                }                
            }
        }

        for(int i = 0; i < 3; i++)
        {
            if(farthestSpawners[i].GetComponent<TimedSpawner>() == null)
            {
                Debug.Log("Unassigned farthest at : " + i);
            } else
            {
                Debug.Log("Assigned farthest at: " + i);
            }
        }
        return farthestSpawners;
    }

    public void SpawnEnemy(string whichEnemy, int howMany)
    {
        Debug.Log("Small spawn called");

        //Grab the farthest spawners from the player
        GameObject[] farthestSpawners = getFarSpawners();
        
        //If less than 2 enemys to spawn, just grab the first spawner and pop them out.
        if (howMany <= 2)
        {
            farthestSpawners[0].GetComponent<TimedSpawner>().AddQueue(whichEnemy, howMany);
            return;
        }

        //We pick randomly from the three fathest spawners
        int whichSpawner = Random.Range(1, 4);

        //We will disperse the enemies throughout, so figure out how many to spawn for each and add the leftovers to the first one.
        int leftOver = howMany % 3;
        int hmPerSpawner = howMany / 3;

        //loop through the spawners starting at whichSpawner and continuing through the three. 
        for(int i = 0; i < 3; i++)
        {
            farthestSpawners[(whichSpawner + i) % 3].GetComponent<TimedSpawner>().AddQueue(whichEnemy, hmPerSpawner + leftOver);
            leftOver = 0;
        }
    }

    public void SpawnEnemy(string whichEnemy, string whichEnemy2, string whichEnemy3, int howMany, int howMany2, int howMany3)
    {
        Debug.Log("Long spawn called");
        //Grab the farthest spawners from the player
        GameObject[] farthestSpawners = getFarSpawners();

        if(GameObject.ReferenceEquals(farthestSpawners[0], farthestSpawners[1]))
        {
            Debug.Log("References equal");
        }

        farthestSpawners[0].GetComponent<TimedSpawner>().AddQueue(whichEnemy, howMany);

        farthestSpawners[1].GetComponent<TimedSpawner>().AddQueue(whichEnemy2, howMany2);

        farthestSpawners[2].GetComponent<TimedSpawner>().AddQueue(whichEnemy3, howMany3);

        return; 

        StartCoroutine(farthestSpawners[0].GetComponent<TimedSpawner>().SpawnEnemy(howMany));

        farthestSpawners[1].GetComponent<TimedSpawner>().NewName(whichEnemy2);
        farthestSpawners[1].GetComponent<TimedSpawner>().IsActive = true;

        StartCoroutine(farthestSpawners[1].GetComponent<TimedSpawner>().SpawnEnemy(howMany2));

        farthestSpawners[2].GetComponent<TimedSpawner>().NewName(whichEnemy3);
        farthestSpawners[2].GetComponent<TimedSpawner>().IsActive = true;

        StartCoroutine(farthestSpawners[2].GetComponent<TimedSpawner>().SpawnEnemy(howMany3));

    }


}
