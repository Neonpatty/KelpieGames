using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWithinBox : MonoBehaviour
{
    public List<FishObject> weightedValues;
    public int fishToSpawn;
    
    [System.Serializable]
    public class FishObject
    {
        public FishFlock PrefabToSpawn;
        public int Weight;
    }
    // Start is called before the first frame update
    void Awake()
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

            var newFish = Instantiate(GetWeightedFish(weightedValues));

            newFish.transform.position = bounds.center + new Vector3(offsetX, offsetY, offsetZ);
            newFish.transform.rotation = rot;

            newFish.SetOrigin(this);
            newFish.SetManager(GF);
            GF.allFish.Add(newFish);
        }
    }

    FishFlock GetWeightedFish(List<FishObject> weightedValueList)
    {
        FishFlock output = null;

        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.Weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.Weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry.PrefabToSpawn;
                break;
            }
        }

        return output;
    }

}
