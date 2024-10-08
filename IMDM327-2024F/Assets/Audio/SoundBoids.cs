// Unity Sound of Boids.
// IMDM 327 Class Material 
// Author: Myungin Lee
// About: Each gameobject generates spatial audio from there position based on their velocity

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoids : MonoBehaviour
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
    float panL, panR = 1; // stereo audio panning. 0 is the center

    void Start()
    {
        // Add audio to the gameobject
        audioSource = gameObject.AddComponent<AudioSource>();
        frequency = (Camera.CameraPosition - gameObject.transform.position).sqrMagnitude / 100f;
        // Stereo panning of refering to the cam pos and boids body
        float theta = CalculatePan(Camera.CameraPosition, gameObject.transform.position);
        // ex) Sin panning http://gdsp.hf.ntnu.no/lessons/1/5/
        panL = Mathf.Sin(theta); 
        panR = Mathf.Cos(theta);

        amplitude = 0.01f; // inital amplitude. set it small since we have hundreds of it.
        tempo = 1000;
        carMul = 1; modMul = 1;
    }

    void Update()
    {
        //for (int i = 0; i < Boids2.numberOfSphere; i++)
        //{
        //    frequency = Boids2.bp[i].acceleration.sqrMagnitude;
        //    pan = (Camera.CameraPosition - Boids2.body[i].transform.position).x;
        //}
        frequency = (Camera.CameraPosition - gameObject.transform.position).sqrMagnitude / 100f;
        // Stereo panning of refering to the cam pos and boids body
        float theta = CalculatePan(Camera.CameraPosition, gameObject.transform.position);
        // ex) Sin panning http://gdsp.hf.ntnu.no/lessons/1/5/
        panL = Mathf.Sin(theta);
        panR = Mathf.Cos(theta);
        // Update the modulation factor according to the distance from camera
        modMul = (Camera.CameraPosition - gameObject.transform.position).sqrMagnitude / 100f;
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
            if (!audioSource.isPlaying)
            {
                timeIdx = 0;  //resets timer before playing sound
                audioSource.Play();
            }
        }
    }

    // This part fill out an array called "data" and send it as a audio buffer
    // So should be careful to assign the computation.
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Button)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                // 
                phase += 2 * Mathf.PI * frequency / sampleRate;
                float source = amplitude * FM(phase, carMul, modMul);
                data[i] = source * panL;
                data[i + 1] = source * panR; // for now, right channel = left channel
                if (phase >= 2 * Mathf.PI)
                {
                    phase -= 2 * Mathf.PI;
                }
                timeIdx++;
            }
        }
    }

    public float CalculatePan(Vector3 Camera, Vector3 Object)
    {
        float theta;
        theta = Mathf.Atan( (Camera - Object).x / (Camera - Object).z );
        return theta;
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
