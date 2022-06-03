 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oscillator : MonoBehaviour
{
    [SerializeField]
    WAVETYPE type;

    [SerializeField]
    float freq = 440f;
    public float currentFreq;
    float increment;
    float phase;
    float samplingFreq = 48000f;

    public int currentOctave;
    public int oldOctave;

    [SerializeField]
    List<GameObject> keys;

    [SerializeField]
    float volume = 0.1f;

    float gain;

    GameObject[] points;

    float[] globalData;

    LineRenderer line;
    Vector3[] linePos;

    wave[] waveFunctions;

    [SerializeField]
    bool[] keybools;

    [SerializeField]
    bool useEnv;
    
    delegate float wave(float t);

    public GameObject sliddy;
    Slider slider;

    Envelope env;

    void Start() {
        points = new GameObject[200];
        globalData = new float[200];
        linePos = new Vector3[200];
        keybools = new bool[18];

        waveFunctions = new wave[] {Waves.Sin, Waves.Square, Waves.Triangle, Waves.Saw};

        currentOctave = 1;
        line = gameObject.GetComponent<LineRenderer>();

        slider = sliddy.GetComponent<Slider>();

        env = new Envelope(0f, 1f, 0f, 0f);
    }
    void Update() 
    {
        currentFreq = freq;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyDown(KeyCode.Quote) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.LeftBracket) || Input.GetKeyDown(KeyCode.RightBracket)) {
            if (useEnv)
                env.keyPressed();
            else {
                gain = volume;
            }
        } else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.G) || Input.GetKeyUp(KeyCode.H) || Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L) || Input.GetKeyUp(KeyCode.Semicolon) || Input.GetKeyUp(KeyCode.Quote) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.R) || Input.GetKeyUp(KeyCode.T) || Input.GetKeyUp(KeyCode.U) || Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.LeftBracket) || Input.GetKeyUp(KeyCode.RightBracket)) {
            if (useEnv)
                env.keyReleased();
            else {
                gain = 0;
            }
        }
        
        if (Input.GetKey(KeyCode.A)) {
            
            freq = 110f;
            keys[0].GetComponent<Image>().color = Color.grey;
        } else if (Input.GetKey(KeyCode.S)) {
            
            freq = 123.47f;
            keys[1].GetComponent<Image>().color = Color.grey;
        } else if(Input.GetKey(KeyCode.D)) {
            
            freq = 130.81f;
            keys[2].GetComponent<Image>().color = Color.grey;
        } else if(Input.GetKey(KeyCode.F)) {
            
            freq = 146.83f;
            keys[3].GetComponent<Image>().color = Color.grey;
        }  else if(Input.GetKey(KeyCode.G)) {
            
            freq = 164.81f;
            keys[4].GetComponent<Image>().color = Color.grey;
        } else if(Input.GetKey(KeyCode.H)) {
            
            freq = 174.61f;
            keys[5].GetComponent<Image>().color = Color.grey;
        }  else if(Input.GetKey(KeyCode.J)) {
            
            freq = 196f;
            keys[6].GetComponent<Image>().color = Color.grey;
        }  else if(Input.GetKey(KeyCode.K)) {
            
            freq = 220f;
            keys[7].GetComponent<Image>().color = Color.grey;
        } else if (Input.GetKey(KeyCode.L)) {
            
            freq = 246.94f;
            keys[8].GetComponent<Image>().color = Color.grey;
        } else if(Input.GetKey(KeyCode.Semicolon)) {
            
            freq = 261.63f;
            keys[9].GetComponent<Image>().color = Color.grey;
        } else if(Input.GetKey(KeyCode.Quote)) {
            
            freq = 293.67f;
            keys[10].GetComponent<Image>().color = Color.grey;
        }  else if(Input.GetKey(KeyCode.W)) {
            
            freq = 116.54f;
            keys[11].GetComponent<Image>().color = Color.black;
        } else if(Input.GetKey(KeyCode.R)) {
            
            freq = 138.59f;
            keys[12].GetComponent<Image>().color = Color.black;
        }  else if(Input.GetKey(KeyCode.T)) {
            
            freq = 155.56f; 
            keys[13].GetComponent<Image>().color = Color.black;
        }  else if(Input.GetKey(KeyCode.U)) {
            
            freq = 185.00f;
            keys[14].GetComponent<Image>().color = Color.black;
        }  else if(Input.GetKey(KeyCode.I)) {
            
            freq = 207.65f;
            keys[15].GetComponent<Image>().color = Color.black;
        } else if(Input.GetKey(KeyCode.O)) {
            
            freq = 233.08f;
            keys[16].GetComponent<Image>().color = Color.black;
        }else if(Input.GetKey(KeyCode.LeftBracket)) {
            
            freq = 277.18f;
            keys[17].GetComponent<Image>().color = Color.black;
        } else if(Input.GetKey(KeyCode.RightBracket)) {
            
            freq = 311.13f;
            keys[18].GetComponent<Image>().color = Color.black;
        }
        else {
            for (var i = 0; i < keys.Count; i++) {
                keys[i].GetComponent<Image>().color = Color.white;
            }
        }

        if (useEnv)
            gain = volume * env.getAmp(Time.time);

        currentFreq = freq * Mathf.Pow(2, currentOctave);

        //visualize
        for (int i = 0; i < 200; i++) {
            linePos[i] = new Vector3(i - 100, globalData[i] * 175, 0);
        }

        line.positionCount = 199;
        line.SetPositions(linePos);
        
        currentOctave = (int)slider.value;
    }

    

    void OnAudioFilterRead(float[] data, int channels) {
        //test
        increment = currentFreq * 2f * Mathf.PI / samplingFreq;

        for (int i = 0; i < data.Length; i += channels) {  
            phase += increment;
            data[i] = gain * waveFunctions[(int)type](phase);

            if (channels == 2) {
                data[i + 1] = data[i];
            }

            if (phase > 2f * Mathf.PI) {
                phase = 0;
            }
        }
        
        for (int i = 0; i < 200; i += channels) {
            globalData[i] = data[i];

            if (channels == 2) {
                globalData[i + 1] = data[i];
            }
        }

        //for harrison: loop through the data[] array, each value is the y value so just graph that
    }

    //knob events
    public void onVolumeChange(float value) {
        Debug.Log(value);
    }

    enum WAVETYPE {
        SIN, SQUARE, TRIANGLE, SAW
    }
}

