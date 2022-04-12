using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JamesNamespace
{
    public class ItemHandler : MonoBehaviour
    {
        public Bait BaitItem;
        public ScreenshotHandler ScreenshotHandler;
        public Net NetItem;

        private Dictionary<int, Items> _items;
        private Items HeldItem = null;

        void Awake()
        {
            HeldItem = ScreenshotHandler;

            _items = new Dictionary<int, Items>();
            _items.Add(0, BaitItem);
            _items.Add(1, ScreenshotHandler);
            _items.Add(2, NetItem);
        }

        void Update()
        {
            ChangeHeldEquipment();

            if (Input.GetMouseButtonDown(0))
            {
                HeldItem.UseAbility(Camera.main.transform);
            }
        }

        void ChangeHeldEquipment()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.E))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex >= _items.Count - 1) EquipItem(0);
                else EquipItem(itemIndex + 1);

                Debug.Log("Equipped Item: " + HeldItem);
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Q))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex <= 0) EquipItem(_items.Count - 1);
                else EquipItem(itemIndex - 1);

                Debug.Log("Equipped Item: " + HeldItem);
            }
        }

        void EquipItem(int index)
        {
            HeldItem = _items[index];
        }

        int GetCurrentItemIndex(Items heldItem)
        {
            foreach(var item in _items)
            {
                if (item.Value == heldItem) return item.Key;
            }

            Debug.LogWarning("No Item on Hand!");
            return 0;
        }
    }
}

