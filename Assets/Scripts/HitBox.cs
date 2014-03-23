using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour 
{
	public Moving myPlayer;

	void Update()
	{
		transform.localPosition = Vector2.zero;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "LaunchAttack" && other.transform != transform.parent)
		{
			myPlayer.HitCollide();
		}
	}

}
