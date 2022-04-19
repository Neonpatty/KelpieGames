using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JamesNamespace
{
    public class ItemHandler : MonoBehaviour
    {
        public static ItemHandler Instance { get; private set; }

        public List<ScriptableItem> AllItemsList;
        private Dictionary<int, ScriptableItem> _items;
        private ScriptableItem HeldItem = null;

        [Space]
        [Header("UI Elements in Scene")]

        [SerializeField] RawImage _cameraImage;
        public GameObject _cameraFrame, itemWheel, _captureImage;
        public Image SlotEquipped, SlotLower, SlotUpper;

        public bool isAimingCamera { get; private set; }

        void Awake()
        {
            Instance = this;

            _items = new Dictionary<int, ScriptableItem>();

            for (int i = 0; i < AllItemsList.Count; i++)
            {
                _items.Add(i, AllItemsList[i]);
            }
            HeldItem = _items[0];
            UpdateIcons();
            _cameraImage.enabled = false;
        }

        void Update()
        {
            ChangeHeldEquipment();

            if (Input.GetMouseButtonDown(0))
            {
                HeldItem.Script.UseAbility(Camera.main, _cameraImage, this);
            }

            if (Input.GetMouseButton(1) && IsPlayerHoldingCamera())
            {
                isAimingCamera = true;
            }
            else
            {
                isAimingCamera = false;
            }
            ChangeCameraState(isAimingCamera);
        }

        void ChangeCameraState(bool isAimingCamera)
        {
            switch (isAimingCamera)
            {
                case true:
                    Camera.main.fieldOfView = 25;
                    _cameraFrame.SetActive(true);
                    itemWheel.SetActive(false);
                    _captureImage.SetActive(false);
                    break;
                case false:
                    Camera.main.fieldOfView = 60;
                    _cameraFrame.SetActive(false);
                    itemWheel.SetActive(true);
                    _captureImage.SetActive(true);
                    break;
            }
        }

        bool IsPlayerHoldingCamera()
        {
            return HeldItem.Name == "Camera" ? true : false;
        }

        void ChangeHeldEquipment()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.E))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex >= _items.Count - 1) EquipItem(0);
                else EquipItem(itemIndex + 1);

                UpdateIcons();
                Debug.Log("Equipped Item: " + HeldItem.Name);
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Q))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex <= 0) EquipItem(_items.Count - 1);
                else EquipItem(itemIndex - 1);

                UpdateIcons();
                Debug.Log("Equipped Item: " + HeldItem.Name);
            }
        }

        void EquipItem(int index)
        {
            HeldItem = _items[index];
        }

        int GetCurrentItemIndex(ScriptableItem heldItem)
        {
            foreach(var item in _items)
            {
                if (item.Value == heldItem) return item.Key;
            }

            Debug.LogWarning("No Item on Hand!");
            return 0;
        }

        void UpdateIcons()
        {
            SlotEquipped.sprite = HeldItem.Icon;

            var itemIndex = GetCurrentItemIndex(HeldItem);

            // IF THIS IS THE HIGHEST ITEM INDEX
            if (itemIndex >= _items.Count - 1) SlotUpper.sprite = _items[0].Icon;
            else SlotUpper.sprite = _items[itemIndex + 1].Icon;

            // IF THIS IS THE LOWEST ITEM INDEX
            if (itemIndex <= 0) SlotLower.sprite = _items[_items.Count - 1].Icon;
            else SlotLower.sprite = _items[itemIndex - 1].Icon;

        }
    }
}

