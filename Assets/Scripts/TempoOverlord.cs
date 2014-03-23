using System;
using UnityEngine;
using System.Collections.Generic;

public class TempoOverlord : MonoBehaviour
{
	public bool Beat;

	private float[] samples = new float[2056];
	private float avgSamples = 0;
	private float rmsValue;
	private float dbValue;
	private float lastValue;

	private int tempo;
	public float beatLength;

	void Start()
	{


	}

	void Update()
	{
		Beat = false;

		audio.GetOutputData (samples, 0);

		foreach(float sample in samples)
		{
			avgSamples += sample * sample;
		}

		rmsValue = Mathf.Sqrt(avgSamples/1024); // rms = square root of average
		dbValue = 20*Mathf.Log10(rmsValue/.01f);

		if(lastValue < -1f)
		{
			if(dbValue > .9f)
			{
				Beat = true;
			}
		}

		lastValue = dbValue;

		avgSamples = 0;
	}

	public void ChangeTempo(int _tempo)
	{
		tempo = _tempo;
		beatLength = 60f/tempo;
	}
}

