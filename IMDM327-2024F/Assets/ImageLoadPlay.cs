using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageLoadPlay : MonoBehaviour
{
    GameObject palette;
    GameObject[] cloud;
    int width, height, decimate;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        // Load image
        string path = "image.png";
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
        Debug.Log(bytes.Length);
        loadTexture.LoadImage(bytes);

        // Get the width and height of the texture
        width = loadTexture.width;
        height = loadTexture.height;

        Color[,] pixelArray = new Color[width, height];
        int numberOfClouds = width * height;
        cloud = new GameObject[numberOfClouds];

        decimate = 20;
        // Loop through each pixel in the texture and store its color in the 2D array
        int i = 0;
        for (int x = 0; x < width/ decimate; x++)
        {
            for (int y = 0; y < height/ decimate; y++)
            {
                i = x * height + y;
                int xf = decimate * x;
                int yf = decimate * y;
                pixelArray[xf, yf] = loadTexture.GetPixel(xf, yf);
                cloud[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); 
                cloud[i].transform.position = new Vector3(x, y, 30);
                cloud[i].GetComponent<Renderer>().material.color = pixelArray[xf, yf];
                cloud[i].transform.localScale = new Vector3(pixelArray[xf, yf].r, pixelArray[xf, yf].g, pixelArray[xf, yf].b);

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        // Loop through each pixel in the texture and store its color in the 2D array
        for (int x = 0; x < width / decimate; x++)
        {
            for (int y = 0; y < height / decimate; y++)
            {
                float speed = cloud[i].transform.localScale.sqrMagnitude;
                i = x * height + y;
                cloud[i].transform.position = new Vector3(x + (1+Mathf.Sin(time*speed+x*0.01f)), y + (1 + Mathf.Cos(time * speed + y * 0.01f)), 30 * (1 + Mathf.Sin(x * speed * 0.001f)) + (1 + Mathf.Cos(time * speed)));

            }
        }
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
