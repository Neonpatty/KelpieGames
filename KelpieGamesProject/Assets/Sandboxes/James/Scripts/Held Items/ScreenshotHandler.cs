using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

namespace JamesNamespace
{
    public class ScreenshotHandler : Items
    {
        public int cameraRange;
        private Coroutine _oldPhotograph;
        RaycastHit m_Hit;
        public override void UseAbility(Camera cam, RawImage camImage, ItemHandler itemHandler)
        {
            if (!ItemHandler.Instance.isAimingCamera) return; // IF NOT AIMING, DONT TAKE PICTURE!!!

            if (_oldPhotograph != null) itemHandler.StopCoroutine(_oldPhotograph);

            itemHandler.StartCoroutine(TakeScreenshot(Screen.width, Screen.height, camImage));

            _oldPhotograph = itemHandler.StartCoroutine(EnableImageInUI(camImage));
        }
        

        private void GetObjectsInBoxCollider(BoxCollider collider, Text textRef)
        {
            Collider[] colliders = Physics.OverlapBox(
                center: collider.transform.position + (collider.transform.rotation * collider.center),
                halfExtents: Vector3.Scale(collider.size * 0.5f, collider.transform.lossyScale),
                orientation: collider.transform.rotation,
                layerMask: ~0);
            List<Transform> objectsInBox = new List<Transform>();

            foreach (var c in colliders)
            {
                Transform t = c.transform;
                while (t.parent != null && t.GetComponent<FishFlock>() == null) t = t.parent;
                if (t.GetComponent<FishFlock>() != null
                    && t != collider.transform
                    && !objectsInBox.Contains(t))
                    objectsInBox.Add(t);
            }

            int i = 0;
            RaycastHit fishInLOS;
            Vector3 dir;

            var environLayer = LayerMask.GetMask("Environment");
            float closestFishDist = 1000;
           // FishFlock closestFishRef;
            collider.enabled = false;
            for (i = 0; i < objectsInBox.Count; i++) //fish found
            {
                dir = objectsInBox[i].transform.position - Camera.main.transform.position;
                var dist = Vector3.Distance(Camera.main.transform.position, objectsInBox[i].transform.position);
                
                Debug.DrawRay(Camera.main.transform.position, dir, Color.red, 10);
                if (Physics.Raycast(Camera.main.transform.position, dir, out fishInLOS, dist, environLayer))
                {
                    print("Wall infront of fish");
                }
                else
                {
                    if(Vector3.Distance(Camera.main.transform.position, objectsInBox[i].transform.position) <= closestFishDist)
                    {
                        closestFishDist = Vector3.Distance(Camera.main.transform.position, objectsInBox[i].transform.position);
                        //closestFishRef = objectsInBox[i];
                        textRef.text = "You captured a: " + objectsInBox[i].GetComponent<FishFlock>().fishName;
                    }
                }
                print(fishInLOS.collider);

            }
            if (i == 0) //no fish found
            {
                textRef.text = "No fishes in range.";
            }
            collider.enabled = true;

        }

        public IEnumerator TakeScreenshot(int width, int height, RawImage camImage)
        {
            var canvas = camImage.GetComponentInParent<Canvas>();
            Text textCanvas = canvas.GetComponentInChildren<Text>();
            print(cameraRange);
            float frustumHeight = 2.0f * cameraRange * Mathf.Tan(12 * 0.5f * Mathf.Deg2Rad);
          
            Camera.main.GetComponent<BoxCollider>().center = new Vector3(0, 0, cameraRange/2);
            Camera.main.GetComponent<BoxCollider>().size = new Vector3(frustumHeight * Camera.main.aspect, frustumHeight, cameraRange);
            GetObjectsInBoxCollider(Camera.main.transform.GetComponent<BoxCollider>(), textCanvas);

            RenderTexture capturedImage = new RenderTexture(width, height, 16);
    
            yield return new WaitForEndOfFrame();
            canvas.enabled = false;
            var volume = GetComponentInChildren<PostProcessVolume>();
            Vignette vingetteRef;
            volume.profile.TryGetSettings(out vingetteRef);
            vingetteRef.active = false;

            ScreenCapture.CaptureScreenshotIntoRenderTexture(capturedImage);

            if (camImage != null)
                camImage.texture = capturedImage;
            else
                Debug.LogError("_takenImage not valid");

            canvas.enabled = true;
            //vingetteRef.active = true;
        }

        public IEnumerator EnableImageInUI(RawImage image)
        {
            image.enabled = true;
            yield return Helpers.GetWait(5f);
            Text textCanvas = image.gameObject.transform.parent.GetComponentInChildren<Text>();
            textCanvas.text = "";
            image.enabled = false;
        }



    }
}

