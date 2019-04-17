using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyDispenser : MonoBehaviour {



    private GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("PlayerPrefab");
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
        GameObject[] allSpawners = GameObject.FindGameObjectsWithTag("Spawner");

        //If the number of spawners found is less than 3, use all 3.
        if(allSpawners.Length <= 3)
        {
            return allSpawners;
        }

        GameObject[] farthestSpawners = new GameObject[3];

        //Compare all of the spawners vs the three saved farthest and replace if possible. 
        for(int i = 0; i < allSpawners.Length; i++)
        {
            
            //This section skips any spawners who are currently in the process of spawning things.    /////////////////Check here if spawners are being used up too quickly, queue system if this becomes an issue.
            if (allSpawners[i].GetComponent<TimedSpawner>().IsActive)
            {
                continue;
            }


            //Loop for the farthest spawners we currently have.
            for(int j = 0;  j < 3; j++)
            {
                //If the current entry we're on is null, tested by checking if it has a 
                //TimedSpawner script. Then replace this entry with the spawner we're on.
                if(farthestSpawners[j].GetComponent<TimedSpawner>() == null)
                {
                    farthestSpawners[j] = allSpawners[i];
                    break;

                    //If it is a valid entry, check if it's closer than the current spawner
                    //we're comparing with, 
                } else
                {
                    if(CompareDistance(farthestSpawners[j], allSpawners[i]) == 0)
                    {
                        //If the new one is farther, save the old one in a temp, and compare that
                        //with the other farthest spawners
                        GameObject temp = farthestSpawners[j];
                        farthestSpawners[j] = allSpawners[i];
                        
                        //This loop checks against the next 1 or 2 entries in the farthestSpawner array
                        for(int n = j+1; n < 3; n++)
                        {
                            //If it's closer than the next farthest, then replace it
                            if (CompareDistance(farthestSpawners[n], temp) == 0)
                            {
                                if (n == 2)
                                {
                                    farthestSpawners[n] = temp;
                                    break;
                                } else
                                {
                                    GameObject secondTemp = farthestSpawners[n];
                                    farthestSpawners[n] = temp;
                                    temp = secondTemp;
                                }
                            }
                        }
                        break;
                    }
                }                
            }
        }
        return farthestSpawners;
    }

    private void spawnEnemy(string whichEnemy, int howMany)
    {

        //Grab the farthest spawners from the player
        GameObject[] farthestSpawners = getFarSpawners();
        
        //If less than 2 enemys to spawn, just grab the first spawner and pop them out.
        if (howMany <= 2)
        {
            farthestSpawners[0].GetComponent<TimedSpawner>().NameToActivate = whichEnemy;
            farthestSpawners[0].GetComponent<TimedSpawner>().Activate(whichEnemy, howMany);
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
            farthestSpawners[(whichSpawner + i)%3].GetComponent<TimedSpawner>().NameToActivate = whichEnemy ;
            farthestSpawners[(whichSpawner + i) % 3].GetComponent<TimedSpawner>().Activate(whichEnemy, howMany + leftOver);
            leftOver = 0;
        }
    }

    private void LevelStart()
    {

    }

}
