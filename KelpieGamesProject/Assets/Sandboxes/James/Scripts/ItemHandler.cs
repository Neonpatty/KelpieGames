using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class ItemHandler : MonoBehaviour
    {
        public Bait BaitItem;

        private Items HeldItem = null;

        void Awake()
        {
            HeldItem = BaitItem;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(HeldItem, transform.position + transform.forward * 2, Quaternion.identity);
            }
        }
    }
}

