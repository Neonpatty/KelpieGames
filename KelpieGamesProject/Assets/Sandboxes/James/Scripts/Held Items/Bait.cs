using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bait : Items
{
    public float UpTime;
    public float Durability;

    public List<FishFlock> Fishes { get; private set; } = new List<FishFlock>();

    public override void UseAbility(Camera cam, RawImage camImage, JamesNamespace.ItemHandler itemHandler)
    {
        Instantiate(this, cam.transform.position + cam.transform.forward * 2, Quaternion.identity);
    }

    void Update()
    {
        if (Fishes.Count <= 0)
        {
            UpTime -= Time.deltaTime;
            if (UpTime <= 0) Destroy(gameObject);
            return;
        }

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
            if (fish.FoundBait || fish.State == FishState.Caught) return;

            fish.FoundBait = this;
            fish.ChangeFishState(FishState.Eating);
        }
    }
}


