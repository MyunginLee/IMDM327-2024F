using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageLoad : MonoBehaviour
{
    GameObject palette;
    // Start is called before the first frame update
    void Start()
    {
        // Load image
        string path = "image.png";
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
        loadTexture.LoadImage(bytes);

        // Generate game object plain
        palette = GameObject.CreatePrimitive(PrimitiveType.Plane); 
        palette.GetComponent<Renderer>().material.mainTexture = loadTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
