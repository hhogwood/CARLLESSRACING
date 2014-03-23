using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifetime;

	void Awake()
	{
		lifetime += Time.time;
	}

	// Update is called once per frame
	void Update () 
	{
		if(lifetime < Time.time)
		{
			GameObject.Destroy(gameObject);
		}
	}
}
