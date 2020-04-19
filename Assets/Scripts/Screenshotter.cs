using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    [SerializeField]
    private string path;

    [SerializeField]
    private float intervalInSeconds;

    int count = 0;
    float timer;

    void Start()
    {
        if (Directory.Exists(path))
            count = Directory.GetFiles(path, "*.png").Length;
        else
            enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= intervalInSeconds)
        {
            timer = 0;
            count ++;
            string filename = $"{path}/screenShot_{count}.png";
            ScreenCapture.CaptureScreenshot(filename);
        }


    }
}
