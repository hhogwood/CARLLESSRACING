﻿using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("hit");

		if(other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<Moving>().HandleDeath();
		}
	}
}