using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameCamera : MonoBehaviour
{

    public WebCamTexture webcamTexture;
    public RawImage rawimage;
    public Color32[] data;
    public move target;
    public move _movered;
    public move _movegreen;
    public Color red;
    public Color green;
    public int redR2;
    public int redG2;
    public int redB2;
    public int redA2;
    public int greenR2;
    public int greenG2;
    public int greenB2;
    public int greenA2;
    int redThreshold = 25;
    int avgRedx;
    int avgRedy;
    int greenThreshold = 25;
    int avgGreenx;
    int avgGreeny;
    int redPixelCount;
    int greenPixelCount;
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        webcamTexture = new WebCamTexture(devices[0].name);
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
        data = new Color32[webcamTexture.width * webcamTexture.height];
    }

    void Update()
    {
        webcamTexture.GetPixels32(data);

        if (Input.GetKeyDown(KeyCode.R))
        {
            float widthFactor = (float) webcamTexture.width / (float) Screen.width;
            float heightFactor = (float) webcamTexture.height / (float) Screen.height;
            redR2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].r;
            redG2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].g;
            redB2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].b;
            redA2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].a;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            float widthFactor = (float)webcamTexture.width / (float)Screen.width;
            float heightFactor = (float)webcamTexture.height / (float)Screen.height;
            greenR2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].r;
            greenG2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].g;
            greenB2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].b;
            greenA2 = data[(int)((widthFactor * Input.mousePosition.x) + (heightFactor * Input.mousePosition.y) * webcamTexture.width)].a;
        }

        avgRedx = 0;
        avgRedy = 0;
        avgGreenx = 0;
        avgGreeny = 0;
        redPixelCount = 0;
        greenPixelCount = 0;

        for (int x = 0; x < (int) webcamTexture.width; x++)
        {
            for (int y = 0; y < (int) webcamTexture.height; y++)
            {
                if (Mathf.Abs(data[x + y * webcamTexture.width].r - redR2) < redThreshold && Mathf.Abs(data[x + y * webcamTexture.width].g - redG2) < redThreshold && Mathf.Abs(data[x + y * webcamTexture.width].b - redB2) < redThreshold && Mathf.Abs(data[x + y * webcamTexture.width].a - redA2) < redThreshold)
                {
                    avgRedx += x;
                    avgRedy += y;
                    redPixelCount += 1;
                }
                if (Mathf.Abs(data[x + y * webcamTexture.width].r - greenR2) < greenThreshold && Mathf.Abs(data[x + y * webcamTexture.width].g - greenG2) < greenThreshold && Mathf.Abs(data[x + y * webcamTexture.width].b - greenB2) < greenThreshold && Mathf.Abs(data[x + y * webcamTexture.width].a - greenA2) < greenThreshold)
                {
                    avgGreenx += x;
                    avgGreeny += y;
                    greenPixelCount += 1;
                }
            }
        }

        //Detect Shot

        /*if (redPixelCount > 0 && greenPixelCount > 0)
        {
            Debug.Log("laser on");
            if ((int)(Mathf.Sqrt(Mathf.Pow((avgRedx / redPixelCount) - (avgGreenx / greenPixelCount), 2) + Mathf.Pow((avgRedy / redPixelCount) - (avgGreeny / greenPixelCount), 2))) < 100)
            {
                target.changePosition((int)(Random.value * 300), (int)(Random.value * 300));
            }
        }*/

        if (redPixelCount > 0)
        {
            _movered.changePosition(avgRedx / redPixelCount, avgRedy / redPixelCount);
        }

        if (greenPixelCount > 0)
        {
            _movegreen.changePosition(avgGreenx / greenPixelCount, avgGreeny / greenPixelCount);
        }
    }

}
