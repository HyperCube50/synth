using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope
{
    private float attack;
    private float decay;
    private float sustain;
    private float release;

    float startTime;
    float releaseTime;

    bool noteOn;

    public Envelope(float a, float d, float s, float r) {
        attack = a;
        decay = d;
        sustain = s;
        release = r;
    }

    public void keyPressed() {
        startTime = Time.time;
        noteOn = false;
    }

    public void keyReleased() {
        releaseTime = Time.time;
        noteOn = false;
    }

    public float getAmp(float t) {
		float amp = 0f;
		float deltaTime = t - startTime;

		if (noteOn)
		{
			if (deltaTime <= attack)
			{
				// In attack Phase - approach max amplitude
				amp = (deltaTime / attack);
			}

			if (deltaTime > attack && deltaTime <= (attack + decay))
			{
				// In decay phase - reduce to sustained amplitude
				amp = ((deltaTime - attack) / decay) * (sustain - 1) + 1;
			}

			if (deltaTime > (attack + decay))
			{
				// In sustain phase - dont change until note released
				amp = sustain;
			}
		}
		else
		{
			// Note has been released, so in release phase
			amp = ((t - releaseTime) / release) * (0f - sustain) + sustain;
		}

		// Amplitude should not be negative
		if (amp <= 0.0001f)
			amp = 0f;

		return amp;
    }

    public void printElapsed(float t) {
        Debug.Log(t - startTime);
    }
}