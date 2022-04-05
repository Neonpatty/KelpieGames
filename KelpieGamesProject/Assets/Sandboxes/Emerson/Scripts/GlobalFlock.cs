using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPrefab; //where the fish are trying to go to
    public static int tankSize = 30; //Size of the play area (meters)
    public static int numFish = 30; //Numb of Fish to spawn
    public static GameObject[] allFish = new GameObject[numFish];

    public static Vector3 goalPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100000) < 50) //this changes the target position of the fish, changes every so oftem
        {
            goalPos = new Vector3(Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize)); //set new goal position for fish to wander to
            goalPrefab.transform.position = goalPos; //sets location of pink sphere
        }
    }
}
