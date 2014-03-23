using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour 
{
	private float delayTime = 0.2f;
	private float destroyTime = 5f;

	void Awake()
	{
		delayTime += Time.time;
		destroyTime += Time.time;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(delayTime < Time.time && other.transform.tag == "Player")
		{
			other.transform.parent.GetComponent<Moving>().Trap();
			GameObject.Destroy(gameObject);
		}
	}

	void Update()
	{
		if (destroyTime < Time.time) GameObject.Destroy (gameObject);
	}
}
