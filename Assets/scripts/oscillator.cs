using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillator : MonoBehaviour
{
    [SerializeField]
    float freq = 440f;
    public float currentFreq;
    float increment;
    float phase;
    float samplingFreq = 48000f;

    public int currentOctave;
    public int oldOctave;

    [SerializeField]
    float volume = 0.1f;

    float gain;

    [SerializeField]
    GameObject datapoint;

    GameObject[] points;

    float[] globalData;
    
    void Start() {
        points = new GameObject[500];
        globalData = new float[500];
        currentOctave = 1;

        for (int i = 0; i < 500; i++) {
            points[i] = Instantiate(datapoint);
        }
    }
    void Update() 
    {
        currentFreq = freq;
        if(Input.GetKey(KeyCode.Space))
        {
            gain = volume;
        } else {
            gain = 0f;
        }
        
        if (Input.GetKey(KeyCode.A)) {
            gain = volume;
            freq = 110f;
        } else if (Input.GetKey(KeyCode.S)) {
            gain = volume;
            freq = 123.47f;
        } else if(Input.GetKey(KeyCode.D)) {
            gain = volume;
            freq = 130.81f;
        } else if(Input.GetKey(KeyCode.F)) {
            gain = volume;
            freq = 146.83f;
        }  else if(Input.GetKey(KeyCode.G)) {
            gain = volume;
            freq = 164.81f;
        } else if(Input.GetKey(KeyCode.H)) {
            gain = volume;
            freq = 174.61f;
        }  else if(Input.GetKey(KeyCode.J)) {
            gain = volume;
            freq = 196f;
        }  else if(Input.GetKey(KeyCode.K)) {
            gain = volume;
            freq = 220f;
        }
        else {
            gain = 0f;
        }

        if(Input.GetKeyDown(KeyCode.X)) {
            currentOctave++;
        }
        if(Input.GetKeyDown(KeyCode.Z)) {
            currentOctave--;
        }
        currentFreq = freq * Mathf.Pow(2, currentOctave);

        //visualize

        for (int i = 0; i < 500; i++) {
            points[i].transform.position = new Vector2(i - 250, globalData[i] * 250);
            if (globalData[i] > globalData[i + 1] + 50) {
                //draw line here
            }
        }
        
    }

    void OnAudioFilterRead(float[] data, int channels) {
        //test
        increment = currentFreq * 2f * Mathf.PI / samplingFreq;

        for (int i = 0; i < data.Length; i += channels) {
            phase += increment;
            data[i] = gain * Waves.Sin(phase);

            if (channels == 2) {
                data[i + 1] = data[i];
            }

            if (phase > 2f * Mathf.PI) {
                phase = 0;
            }
        }
        
        for (int i = 0; i < 500; i += channels) {
            globalData[i] = data[i];

            if (channels == 2) {
                globalData[i + 1] = data[i];
            }
        }

        //for harrison: loop through the data[] array, each value is the y value so just graph that
    }
}

