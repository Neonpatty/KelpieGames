using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
namespace JamesNamespace
{
    public class ItemHandler : MonoBehaviour
    {
        public static ItemHandler Instance { get; private set; }

        [SerializeField] AudioSource _audSource;
        [SerializeField] AudioClip _itemSwapClip;
        public List<ScriptableItem> AllItemsList;
        private Dictionary<int, ScriptableItem> _items;
        private ScriptableItem HeldItem = null;

        [Space]
        [Header("UI Elements in Scene")]

        [SerializeField] RawImage _cameraImage;
        [SerializeField] Text _textRef;

        public GameObject _cameraFrame, itemWheel, _captureImage;
        public Image SlotEquipped, SlotLower, SlotUpper;

        public bool isAimingCamera { get; private set; }

        float targetCameraFov;
        PostProcessVolume volume;
        Vignette vingetteRef;
        float vingetteIntensity;
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
            volume = GameObject.FindObjectOfType<PostProcessVolume>();
            volume.profile.TryGetSettings(out vingetteRef);
        }

        void Update()
        {
            ChangeHeldEquipment();

            if (Input.GetMouseButtonDown(0))
            {
                HeldItem.Script.UseAbility(Camera.main, _cameraImage, this);
                if (IsPlayerHoldingCamera())
                {
                    if (isAimingCamera) _audSource.PlayClip(HeldItem.Audio);
                }
                else _audSource.PlayClip(HeldItem.Audio, Random.Range(-10, 11) / 100);
            }

            if (Input.GetMouseButtonUp(0) && IsPlayerHoldingSeaGlider())
            {
                HeldItem.Script.StopAbility(Camera.main, _cameraImage, this);
                _audSource.Stop();
            }

            if (Input.GetMouseButton(1) && IsPlayerHoldingCamera())
            {
                isAimingCamera = true;
                targetCameraFov = 25;
                vingetteIntensity = 1;
            }
            else
            {
                isAimingCamera = false;
                targetCameraFov = 60;
                vingetteIntensity = 0.8f;

            }
            ChangeCameraState(isAimingCamera);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetCameraFov, 10 * Time.deltaTime);
            vingetteRef.intensity.value = Mathf.Lerp(vingetteRef.intensity.value, vingetteIntensity, 10 * Time.deltaTime);
        }

        void ChangeCameraState(bool isAimingCamera)
        {
            switch (isAimingCamera)
            {
                case true:
                    //Camera.main.fieldOfView = 25;
                    //_cameraFrame.SetActive(true);
                    _textRef.enabled = false; 
                    itemWheel.SetActive(false);
                    _captureImage.SetActive(false);
                    break;
                case false:
                    // Camera.main.fieldOfView = 60;
                    //_cameraFrame.SetActive(false);
                    _textRef.enabled = true;
                    itemWheel.SetActive(true);
                    _captureImage.SetActive(true);
                    break;
            }
        }



        bool IsPlayerHoldingCamera()
        {
            return HeldItem.Name == "Camera" ? true : false;
        }

        bool IsPlayerHoldingSeaGlider()
        {
            return HeldItem.Name == "SeaGlider" ? true : false;
        }

        void ChangeHeldEquipment()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.E))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex >= _items.Count - 1) EquipItem(0);
                else EquipItem(itemIndex + 1);

                float randPitch = Random.Range(-10, 11) / 100;
                _audSource.PlayClip(_itemSwapClip, randPitch);

                UpdateIcons();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Q))
            {
                var itemIndex = GetCurrentItemIndex(HeldItem);

                if (itemIndex <= 0) EquipItem(_items.Count - 1);
                else EquipItem(itemIndex - 1);

                float randPitch = Random.Range(-10, 11) / 100;
                _audSource.PlayClip(_itemSwapClip, randPitch);

                UpdateIcons();
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

