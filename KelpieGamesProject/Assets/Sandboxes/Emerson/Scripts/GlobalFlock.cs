using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public static GlobalFlock Instance;

    void Awake() => Instance = this;

    public FishFlock fishPrefab;
    public Transform goalPrefab; //where the fish are trying to go to
    public int tankSizeX = 50; //Size of the play area (meters)
    [SerializeField] int tankSizeY = 40; //Size of the play area (meters)
    [SerializeField] int tankSizeZ = 50; //Size of the play area (meters)
    [SerializeField] int numFish = 300; //Numb of Fish to spawn
    [SerializeField] int distToCullFish = 50; //how far away should we stop rendering fish?
    [SerializeField] float timeToCullFish = 5f; //how often should we reactivate fish?

    public FishFlock[] allFish { get; private set; }
    public FishFlock[] activeFish { get; private set; }

    public Vector3 goalPos { get; private set; } = Vector3.zero;

    JamesNamespace.SwimmingController playerRef;


    // Start is called before the first frame update
    void Start()
    {
        allFish = new FishFlock[numFish];
        for(int i=0; i< numFish; i++)
        {
            Vector3 pos = RandomPosInCube(tankSizeX, tankSizeY, tankSizeZ);
            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
            
        }
        playerRef = FindObjectOfType<JamesNamespace.SwimmingController>();

        StartCoroutine(OptimiseFishies());

    }

    IEnumerator OptimiseFishies()
    {
        foreach(FishFlock i in allFish)
        {
            if(Vector3.Distance(playerRef.transform.position, i.transform.position) > distToCullFish)
            {
                i.gameObject.SetActive(false);
            }
            else
            {
                i.gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(timeToCullFish);
        StartCoroutine(OptimiseFishies());
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100000) < 50) //this changes the target position of the fish, changes every so oftem
        {
            goalPos = RandomPosInCube(tankSizeX, tankSizeY, tankSizeZ); //set new goal position for fish to wander to
            goalPrefab.position = goalPos; //sets location of pink sphere
        }
    }

    Vector3 RandomPosInCube(int width, int height, int length)
    {
        var offset = transform.position;
        return new Vector3(Random.Range(-width, width),
                Random.Range(-height, height),
                Random.Range(-length, length)) + 
                offset;
    }
    public Vector3 RandomPosInCube_Public()
    {
        return RandomPosInCube(tankSizeX, tankSizeY, tankSizeZ);
    }
}
