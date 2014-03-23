using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour 
{
	void Start () 
	{
		//Overlord.Instance.SO.PlayMusic ("Helix Nebula");
		Overlord.Instance.SO.PlaySound ("get to the top", 1f);
	}

	void Update () 
	{
		if(Overlord.Instance.TO.BeatCount == 28 && Overlord.Instance.TO.Beat)
		{
			Overlord.Instance.SO.PlaySound ("three", 1f);
		}

		if(Overlord.Instance.TO.BeatCount == 29 && Overlord.Instance.TO.Beat)
		{
			Overlord.Instance.SO.PlaySound ("two", 1f);
		}

		if(Overlord.Instance.TO.BeatCount == 30 && Overlord.Instance.TO.Beat)
		{
			Overlord.Instance.SO.PlaySound ("one", 1f);
		}

		if(Overlord.Instance.TO.BeatCount == 31 && Overlord.Instance.TO.Beat)
		{
			Overlord.Instance.SO.PlaySound ("go", 1f);
			GameObject.Find("StartBox").SetActive(false);
		}
	}
}
