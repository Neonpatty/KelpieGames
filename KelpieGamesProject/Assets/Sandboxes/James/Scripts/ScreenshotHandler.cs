using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : Items
{
    [SerializeField] RawImage _takenImage;

    public void TakeScreenshot_Static(int width, int height)
    {
        RenderTexture capturedImage = new RenderTexture(width, height, 16);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(capturedImage);
        if (_takenImage != null)
            _takenImage.texture = capturedImage;
        else
            Debug.LogError("_takenImage not valid");

    }



}
