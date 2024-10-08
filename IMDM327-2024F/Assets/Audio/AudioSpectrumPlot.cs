// Unity Audio Spectrum Plot Example
// IMDM 327 Class Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrumPlot : MonoBehaviour
{
    // Scale the plot
    [Range (1f, 100f)]
    public float scale = 10;

    // frequency bins are intervals between samples in frequency domain
    GameObject[] sampleBin = new GameObject[AudioSpectrum.FFTSIZE]; 

    void Start()
    {
        // For every frequency bin
        for (int i = 0; i < sampleBin.Length; i++) 
        {   // Create GO and init position
            sampleBin[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sampleBin[i].transform.position = new Vector3(i * 0.01f - 5, 0, 0);
        }
    }
    void Update()
    {
        for (int i = 0;i < sampleBin.Length;i++)
        {
            if(sampleBin != null)
            {
                sampleBin[i].transform.localScale = new Vector3(0.01f, AudioSpectrum.samples[i] * scale, 0.1f);
                sampleBin[i].GetComponent<Renderer>().material.color = new Color(0.3f+i/100f, 0.1f + i /300f, 1- i /500f);
            }
        }
        
    }
}
