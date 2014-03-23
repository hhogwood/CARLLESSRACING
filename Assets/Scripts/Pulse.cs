using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour 
{
	private float defaultScale;
	 
	void Start () 
	{
		defaultScale = transform.localScale.x;
	}
	
	void Update()
	{
		if(Overlord.Instance.TO.Beat)
		{
			transform.localScale = new Vector3 (transform.localScale.x + 10f, transform.localScale.y + 10f, transform.localScale.z) ;
		}
		
		if(transform.localScale.x > defaultScale)
		{
			transform.localScale = new Vector3 (transform.localScale.x - .5f, transform.localScale.y - .5f, transform.localScale.z) ;
		}
	}
}
