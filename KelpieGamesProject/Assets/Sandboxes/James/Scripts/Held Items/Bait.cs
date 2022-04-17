using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bait : Items
{
    public float Durability;

    public List<FishFlock> Fishes = new List<FishFlock>();

    public override void UseAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {
        Instantiate(this, cam.transform.position + cam.transform.forward * 2, Quaternion.identity);
    }

    void Update()
    {
        if (Fishes.Count <= 0) return;

        Durability -= Fishes.Count * Time.deltaTime;
        if (Durability <= 0)
        {
            foreach (FishFlock f in Fishes)
            {
                f.FoundBait = null;
                f.ChangeFishState(FishState.Swimming);
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FishFlock fish))
        {
            if (fish.FoundBait) return;

            fish.FoundBait = this;
            fish.ChangeFishState(FishState.Eating);
        }
    }
}


