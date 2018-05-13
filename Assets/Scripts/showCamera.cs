using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showCamera : MonoBehaviour {

    public WebCamTexture webcamTexture;
    public RawImage rawimage;
    public Color32[] data;
    public move _movered;
    public move _movegreen;
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

    //Algorithm 1 (hard coded red and green comparison vals)

    /*void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);
            int highred = 0;
            int highredx = 0;
            int highredy = 0;

            int highgreen = 0;
            int highgreenx = 0;
            int highgreeny = 0;
            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    if (data[x + y * webcamTexture.width].r > highred && data[x + y * webcamTexture.width].g < 50 && data[x + y * webcamTexture.width].b < 50)
                    {
                        highred = data[x + y * webcamTexture.width].r;
                        highredx = x;
                        highredy = y;
                    }
                    if (data[x + y * webcamTexture.width].g > highgreen && data[x + y * webcamTexture.width].r < 50 && data[x + y * webcamTexture.width].b < 50)
                    {
                        highgreen = data[x + y * webcamTexture.width].g;
                        highgreenx = x;
                        highgreeny = y;
                    }
                }
            }
            _movered.changePosition(highredx, highredy);

            _movegreen.changePosition(highgreenx, highgreeny);

        }
    }*/

    //---------------------------------------------------------------------------

    //Algorithm 2 (euclidian color distance)

    /*void Update()
    {
        if (webcamTexture.didUpdateThisFrame) { 
            webcamTexture.GetPixels32(data);

            int redDiff = 255 * 3;
            int redx = 0;
            int redy = 0;

            int greenDiff = 255 * 3;
            int greenx = 0;
            int greeny = 0;

            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    int newRedDiff = colorDistance(data[x + y * webcamTexture.width].r, data[x + y * webcamTexture.width].g, data[x + y * webcamTexture.width].b, 255, 0, 0);
                    if (newRedDiff < redDiff)
                    {
                        redDiff = newRedDiff;
                        redx = x;
                        redy = y;
                    }

                    int newGreenDiff = colorDistance(data[x + y * webcamTexture.width].r, data[x + y * webcamTexture.width].g, data[x + y * webcamTexture.width].b, 0, 255, 0);
                    if (newGreenDiff < greenDiff)
                    {
                        greenDiff = newGreenDiff;
                        greenx = x;
                        greeny = y;
                    }
                }
            }

            _movered.changePosition(redx, redy);
            _movegreen.changePosition(greenx, greeny);

        }
    }

    public static int colorDistance(int r1, int g1, int b1, int r2, int g2, int b2) {
        int diff = (int) (Mathf.Sqrt(Mathf.Pow(r1 - r2, 2) + Mathf.Pow(g1 - g2, 2) + Mathf.Pow(b1 - b2, 2)));
        return diff;
    }*/

    //---------------------------------------------------------------------------

    //Algorithm 3 (track at average place)

    /*void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);

            int redDiffThresh = 75;
            int avgRedx = 0;
            int avgRedy = 0;

            int greenDiffThresh = 75;
            int avgGreenx = 0;
            int avgGreeny = 0;

            int redPixelCount = 0;
            int greenPixelCount = 0;

            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    int newRedDiff = colorDistance(data[x + y * webcamTexture.width].r, data[x + y * webcamTexture.width].g, data[x + y * webcamTexture.width].b, 255, 0, 0);
                    if (newRedDiff < redDiffThresh)
                    {
                        avgRedx += x;
                        avgRedy += y;
                        redPixelCount += 1;
                    }

                    int newGreenDiff = colorDistance(data[x + y * webcamTexture.width].r, data[x + y * webcamTexture.width].g, data[x + y * webcamTexture.width].b, 0, 255, 0);
                    if (newGreenDiff < greenDiffThresh)
                    {
                        avgGreenx += x;
                        avgGreeny += y;
                        greenPixelCount += 1;
                    }
                }
            }

            if (redPixelCount > 0) {
                _movered.changePosition(avgRedx / redPixelCount, avgRedy / redPixelCount);
            }

            if (greenPixelCount > 0) {
                _movegreen.changePosition(avgGreenx / greenPixelCount, avgGreeny / greenPixelCount);
            }

        }
    }

    public static int colorDistance(int r1, int g1, int b1, int r2, int g2, int b2)
    {
        int diff = (int)(Mathf.Sqrt(Mathf.Pow(r1 - r2, 2) + Mathf.Pow(g1 - g2, 2) + Mathf.Pow(b1 - b2, 2)));
        return diff;
    }*/

    //---------------------------------------------------------------------------

    //Algorithm 4 (hardcoded average tracking)

    void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);
            int redThreshold = 25;
            int avgRedx = 0;
            int avgRedy = 0;

            int greenThreshold = 25;
            int avgGreenx = 0;
            int avgGreeny = 0;

            int redPixelCount = 0;
            int greenPixelCount = 0;

            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    if (data[x + y * webcamTexture.width].r > 255 - redThreshold)
                    {
                        avgRedx += x;
                        avgRedy += y;

                        redPixelCount += 1;
                    }
                    if (data[x + y * webcamTexture.width].g > 255 - greenThreshold)
                    {
                        avgGreenx += x;
                        avgGreeny += y;

                        greenPixelCount += 1;
                    }
                }
            }

            if (redPixelCount > 0) {
                _movered.changePosition(avgRedx / redPixelCount, avgRedy / redPixelCount);
            }

            if (greenPixelCount > 0) {
                _movegreen.changePosition(avgGreenx / greenPixelCount, avgGreeny / greenPixelCount);
            }
        }
    }

    //---------------------------------------------------------------------------

    //Algorithm 5

    /*void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);
            int redThreshold = 25;
            int avgRedx = 0;
            int avgRedy = 0;

            int greenThreshold = 25;
            int avgGreenx = 0;
            int avgGreeny = 0;

            int redPixelCount = 0;
            int greenPixelCount = 0;

            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    if ((int)(Mathf.Sqrt(Mathf.Pow(data[x + y * webcamTexture.width].r - 255, 2) + Mathf.Pow(data[x + y * webcamTexture.width].g - 0, 2) + Mathf.Pow(data[x + y * webcamTexture.width].b - 0, 2))) < redThreshold)
                    {
                        avgRedx += x;
                        avgRedy += y;

                        redPixelCount += 1;
                    }
                    if ((int)(Mathf.Sqrt(Mathf.Pow(data[x + y * webcamTexture.width].r - 0, 2) + Mathf.Pow(data[x + y * webcamTexture.width].g - 255, 2) + Mathf.Pow(data[x + y * webcamTexture.width].b - 0, 2))) < greenThreshold)
                    {
                        avgGreenx += x;
                        avgGreeny += y;

                        greenPixelCount += 1;
                    }
                }
            }

            if (redPixelCount > 0)
            {
                _movered.changePosition(avgRedx / redPixelCount, avgRedy / redPixelCount);
            }

            if (greenPixelCount > 0)
            {
                _movegreen.changePosition(avgGreenx / greenPixelCount, avgGreeny / greenPixelCount);
            }
        }
    }*/

    //---------------------------------------------------------------------------

    //Algorithm 6

    /*void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);
            int redThreshold = 25;
            int avgRedx = 0;
            int avgRedy = 0;

            int greenThreshold = 25;
            int avgGreenx = 0;
            int avgGreeny = 0;

            int redPixelCount = 0;
            int greenPixelCount = 0;

            for (int x = 0; x < (int)webcamTexture.width; x++)
            {
                for (int y = 0; y < (int)webcamTexture.height; y++)
                {
                    if (data[x + y * webcamTexture.width].r > 255 - redThreshold && data[x + y * webcamTexture.width].g < 100 && data[x + y * webcamTexture.width].b < 100)
                    {
                        avgRedx += x;
                        avgRedy += y;

                        redPixelCount += 1;
                    }
                    if (data[x + y * webcamTexture.width].g > 255 - greenThreshold && data[x + y * webcamTexture.width].r < 100 && data[x + y * webcamTexture.width].b < 100)
                    {
                        avgGreenx += x;
                        avgGreeny += y;

                        greenPixelCount += 1;
                    }
                }
            }

            if (redPixelCount > 0)
            {
                _movered.changePosition(avgRedx / redPixelCount, avgRedy / redPixelCount);
            }

            if (greenPixelCount > 0)
            {
                _movegreen.changePosition(avgGreenx / greenPixelCount, avgGreeny / greenPixelCount);
            }
        }
    }*/
}
