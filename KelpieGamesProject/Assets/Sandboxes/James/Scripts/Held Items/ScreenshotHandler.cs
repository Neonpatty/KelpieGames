using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JamesNamespace
{
    public class ScreenshotHandler : Items
    {
        private Coroutine _oldPhotograph;

        public override void UseAbility(Camera cam, RawImage camImage, ItemHandler itemHandler)
        {
            if (!ItemHandler.Instance.isAimingCamera) return; // IF NOT AIMING, DONT TAKE PICTURE!!!

            if (_oldPhotograph != null) itemHandler.StopCoroutine(_oldPhotograph);

            itemHandler.StartCoroutine(TakeScreenshot(Screen.width, Screen.height, camImage));

            _oldPhotograph = itemHandler.StartCoroutine(EnableImageInUI(camImage));
        }
        public IEnumerator TakeScreenshot(int width, int height, RawImage camImage)
        {
            RenderTexture capturedImage = new RenderTexture(width, height, 16);
            var canvas = camImage.GetComponentInParent<Canvas>();


            yield return new WaitForEndOfFrame();
            canvas.enabled = false;

            ScreenCapture.CaptureScreenshotIntoRenderTexture(capturedImage);

            if (camImage != null)
                camImage.texture = capturedImage;
            else
                Debug.LogError("_takenImage not valid");

            canvas.enabled = true;

        }

        public IEnumerator EnableImageInUI(RawImage image)
        {
            image.enabled = true;
            yield return Helpers.GetWait(2f);
            image.enabled = false;
        }



    }
}

