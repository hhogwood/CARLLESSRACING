using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundOverlord : MonoBehaviour
{
	private Queue<AudioSource> available = new Queue<AudioSource>();
	private List<AudioSource> unavailable = new List<AudioSource>();
	private Queue<bool> points = new Queue<bool>();
	private int pointsScale = 1;
	private int scaleFrameCount;
	private bool muted = false;

	AudioSource music;
	AudioSource playerDamage;
	AudioLowPassFilter lpf;

	void Start()
	{
		#region AudioSources
		AudioSource audio1 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio2 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio3 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio4 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio5 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio6 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio7 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio8 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio9 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio10 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio11 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio12 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio13 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio14 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio15 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio16 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio17 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio18 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio19 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		AudioSource audio20 = GameObject.Find("SoundOverlord").AddComponent<AudioSource>() as AudioSource;
		#endregion

		music = GameObject.Find("Music").AddComponent<AudioSource>() as AudioSource;
		music.loop = true;

		available.Enqueue(audio1);
		available.Enqueue(audio2);
		available.Enqueue(audio3);
		available.Enqueue(audio4);
		available.Enqueue(audio5);
		available.Enqueue(audio6);
		available.Enqueue(audio7);
		available.Enqueue(audio8);
		available.Enqueue(audio9);
		available.Enqueue(audio10);
		available.Enqueue(audio11);
		available.Enqueue(audio12);
		available.Enqueue(audio13);
		available.Enqueue(audio14);
		available.Enqueue(audio15);
		available.Enqueue(audio16);
		available.Enqueue(audio17);
		available.Enqueue(audio18);
		available.Enqueue(audio19);
		available.Enqueue(audio20);
	}

	void Update()
	{
		for(int i=0; i < unavailable.Count; i++)
		{
			if(unavailable[i].isPlaying == false)
			{
				unavailable[i].clip = null;
				available.Enqueue(unavailable[i]);
				unavailable.RemoveAt(i);
				i--;
			}
		}

		if(Input.GetKeyDown(KeyCode.M))
		{
			if(!muted)
			{
				Debug.Log("mute");
				foreach(AudioSource source in available)
				{
					source.mute = true;
				}

				music.mute = true;

				muted = true;
			}

			else if(muted)
			{
				Debug.Log("Unmute");
				foreach(AudioSource source in available)
				{
					source.mute = false;
				}
				
				music.mute = false;

				muted = false;
			}
		}
	}

	public void PlaySound(string _soundName, float _volume)
	{
		if(available.Count != 0)
		{
			AudioSource source = available.Dequeue ();
			unavailable.Add (source);
			source.clip = Resources.Load("Audio/SFX/" + _soundName) as AudioClip;
			source.volume = _volume;
			source.Play ();
		}
	}

	public void PlaySound(string _soundName, float _volume, float _delay)
	{
		AudioSource source = available.Dequeue ();
		unavailable.Add (source);
		source.clip = Resources.Load("Audio/SFX/" + _soundName) as AudioClip;
		source.volume = _volume;
		source.PlayDelayed (_delay);
	}

	public void PlayMusic(string _level)
	{
		music.clip = Resources.Load("Audio/Music/" + _level) as AudioClip;
		music.Play ();
		GameObject.Find("Manager").audio.Play ();
	}

	public void StopMusic()
	{
		music.Stop ();
		music.clip = null;
	}
}