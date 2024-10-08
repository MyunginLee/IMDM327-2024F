// Unity Audio Spectrum data analysis
// IMDM 327 Class Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
    AudioSource source;
    public static int FFTSIZE = 1024; // https://en.wikipedia.org/wiki/Fast_Fourier_transform
    public static float[] samples = new float[FFTSIZE];
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();       
    }

    // Update is called once per frame
    void Update()
    {
        // The source (time domain) transforms into samples in frequency domain 
        GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.Hanning);
        
    }
}
