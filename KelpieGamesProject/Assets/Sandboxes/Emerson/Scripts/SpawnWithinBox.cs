using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWithinBox : MonoBehaviour
{
    public List<objectsToSpawn> weightedValues;
    public int fishToSpawn;
    
    [System.Serializable]
    public class objectsToSpawn
    {
        public GameObject GameObjectToSpawn;
        public int weight;
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalFlock GF = FindObjectOfType<GlobalFlock>();
        print(GF);
        for(int i = 0; i <= fishToSpawn; i++)
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
            float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject newFish = GameObject.Instantiate(GetRandomValue(weightedValues));

            newFish.transform.position = bounds.center + new Vector3(offsetX, offsetY, offsetZ);
            newFish.transform.rotation = rot;

            newFish.GetComponent<FishFlock>().SetOrigin(GF);
            GF.allFish.Add(newFish.GetComponent<FishFlock>());
        }
    }

    GameObject GetRandomValue(List<objectsToSpawn> weightedValueList)
    {
        GameObject output = null;

        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry.GameObjectToSpawn;
                break;
            }
        }

        return output;
    }
    
    

    


}
