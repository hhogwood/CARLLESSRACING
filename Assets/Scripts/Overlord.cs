using System;
using UnityEngine;
using System.Collections.Generic;

public class Overlord : MonoBehaviour
{
	private static Overlord instance = null;
	public static Overlord Instance { get { return instance; } }

	public TempoOverlord TO;
	public SoundOverlord SO;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		TO = gameObject.GetComponent<TempoOverlord>();
		SO = GameObject.Find("SoundOverlord").GetComponent<SoundOverlord>();
	}
}
