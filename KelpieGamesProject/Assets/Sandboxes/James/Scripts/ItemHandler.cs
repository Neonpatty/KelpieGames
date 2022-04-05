using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JamesNamespace
{
    public class ItemHandler : MonoBehaviour
    {
        public Bait BaitItem;
        public ScreenshotHandler ScreenshotHandler;

        private Items HeldItem = null;

        void Awake()
        {
            HeldItem = ScreenshotHandler;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (HeldItem == BaitItem)
                {
                    Instantiate(HeldItem, transform.position + transform.forward * 2, Quaternion.identity);
                }
                else if (HeldItem == ScreenshotHandler)
                {
                    ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
                    
                }
                
            }
        }
    }
}

