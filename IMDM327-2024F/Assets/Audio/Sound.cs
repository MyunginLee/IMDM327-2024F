// Unity Audio Synthesis (Sine + FM + Envelope)
// IMDM 327 Class Material 
// Author: Myungin Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource audioSource;

    [Range(20, 20000)]  //Creates a slider in the inspector
    public float frequency;
    [Range(100f, 3000f)]  //Creates a slider in the inspector
    public float tempo;

    [Range(0f, 10f)]  //Creates a slider in the inspector
    public float carMul;
    [Range(0f, 10f)]  //Creates a slider in the inspector
    public float modMul;

    public float sampleRate = 44100f;
    float timeIdx= 0;
    [Range(0.1f, 2)]  //Creates a slider in the inspector
    public float amplitude;
    float phase = 0; // phase of an oscillator. If many, this also should be an array
    private bool Button = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        frequency = 440f; // inital frequency
        amplitude = 0.5f; // inital amplitude
        tempo = 1000;
        carMul = 1; modMul = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
            if (!audioSource.isPlaying)
            {
                timeIdx = 0;  //resets timer before playing sound
                audioSource.Play();
            }
        }
        // turn off the audio when the envelope is small enough.
        if (timeIdx > 1000 && Envelope(timeIdx, tempo) < 0.001)
        {
            audioSource.Stop();
            timeIdx = 0;
            Button= !Button;
        }
    }

    // This part fill out an array called "data" and send it as a audio buffer
    // ** This function runs "sampleRate" times a second.
    // So should be careful to assign the computation.
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Button)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                // 
                phase += 2 * Mathf.PI * frequency / sampleRate;
//                data[i] = amplitude * SinWave(phase) * Envelope(timeIdx, tempo);
                data[i] = amplitude * FM(phase, carMul, modMul) * Envelope(timeIdx, tempo);
                data[i + 1] = data[i]; // for now, right channel = left channel
                if (phase >= 2 * Mathf.PI)
                {
                    phase -= 2 * Mathf.PI;
                }
                timeIdx++;
            }
        }
    }

    public float SinWave(float phase)
    {
        return Mathf.Sin(phase); 
    }

    // Frequency Modulation computation
    public float FM(float phase, float carMul, float modMul)
    {
        return Mathf.Sin(phase * carMul) + Mathf.Sin(phase * modMul); // fluctuating FM
   }
    public float Envelope(float timeIdx, float tempo)
    {   // should have something looks like..: /\__
        // https://www.sciencedirect.com/topics/engineering/envelope-function
        float a = 0.13f;
        float b = 0.45f;
        return Mathf.Abs(Mathf.Exp(-a * (timeIdx)/tempo) - Mathf.Exp(-b * (timeIdx)/ tempo ));
    }
}
