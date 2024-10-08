using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Frequency Modulation Synthesizer using OSC message
public class AudioOSC : MonoBehaviour
{
    [SerializeField] private OSC osc;
    // audio setup
    static public float sampleRate = 44100;
    [Range(0.4f, 2)]  //Creates a slider in the inspector
    public float masterAmp; // amplitude of audio
    AudioSource audioSource;

    // audio variables
    static int numberOfNotes = 3;
    [Range(523f, 783f)]
    float[] frequency = new float[numberOfNotes]; // main note frequency
    float[] phase = new float[numberOfNotes];
    float[] amplitude = new float[numberOfNotes];
    private bool Button = false;
    public float OSCinput;
    // FM (maybe)
    [Range(0.0f, 2.0f)]
    public float midnoteMultiplier; // 
    [Range(0, 20)]
    public float carrierMultiplier; // carrier frequency = frequency * carrierMultiplier
    [Range(0, 20)]
    public float modularMultiplier; // modular frequency = frequency * modularMultiplier

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        carrierMultiplier = 1.005f;
        modularMultiplier = 1;
        midnoteMultiplier = 1;
        masterAmp = 0.75f; // audible but small init
        frequency[0] = 523.25f;
        frequency[1] = 659.25f;
        frequency[2] = 783.99f;

        // Set OSC label
        osc.SetAddressHandler("/fromPython", OnMessageReceiveData);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
        }
        midnoteMultiplier = OSCinput * 2;
        Debug.Log(OSCinput);
        frequency[0] = 523.25f;
        frequency[1] = 659.25f * midnoteMultiplier;
        frequency[2] = 783.99f;

        amplitude[0] = 0.1f;
        amplitude[1] = 0.3f;
        amplitude[2] = 0.1f;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Button)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                float[] freqinphase = new float[numberOfNotes];
                for (int j = 0; j < numberOfNotes; j++)
                {
                    freqinphase[j] = Mathf.PI * frequency[j] / sampleRate;
                }
                phase[0] += freqinphase[0]; // each note has different phase
                phase[1] += freqinphase[1];
                phase[2] += freqinphase[2];

                // stack chords
                float chords = 0;
                chords += amplitude[0] * FM(phase[0], 1, 1);
                chords += amplitude[1] * FM(phase[1], carrierMultiplier, modularMultiplier);
                chords += amplitude[2] * FM(phase[2], 1, 1);

                data[i] = masterAmp * chords;
                data[i + 1] = data[i];

                // Rollback phase
                for (int j = 0; j < numberOfNotes; j++)
                {
                    if (phase[j] >= 2 * Mathf.PI)
                    {
                        phase[j] -= 2 * Mathf.PI;
                    }
                }
            }
        }
    }

    // Frequency Modulation computation
    public float FM(float phase, float carMul, float modMul)
    {
        return Mathf.Sin(phase * carMul + Mathf.Sin(phase * modMul)); // fluctuating FM
    }

    void OnMessageReceiveData(OscMessage message)
    {
        float input = message.GetFloat(0);
        OSCinput = input;
        //Debug.Log(OSCinput);
    }
}

