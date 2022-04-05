using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class Bait : Items
    {
        public float Durability;

        public List<FishMovement> Fishes = new List<FishMovement>();

        void Update()
        {
            Durability -= Fishes.Count * Time.deltaTime;

            if (Durability <= 0)
            {
                Durability = 0;

                foreach (FishMovement fish in Fishes)
                {
                    fish.FoundBait = null;
                }
                Fishes.Clear();
                Destroy(gameObject);
            }
        }
    }

}
