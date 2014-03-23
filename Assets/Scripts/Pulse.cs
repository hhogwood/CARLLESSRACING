using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour 
{
	public float ScaleUp;
	public float ScaleDown;

	private float defaultScaleX;
	private float defaultScaleY;
	 
	void Start () 
	{
		defaultScaleX = transform.localScale.x;
		defaultScaleY = transform.localScale.y;
	}

	void Update()
	{
		if(Overlord.Instance.TO.Beat)
		{
			transform.localScale = new Vector3 (transform.localScale.x + ScaleUp, transform.localScale.y + ScaleUp, transform.localScale.z) ;
		}

		if(transform.localScale.x > defaultScaleX)
		{
			transform.localScale = new Vector3 (transform.localScale.x - ScaleDown, transform.localScale.y - ScaleDown, transform.localScale.z) ;
		}

		if(transform.localScale.x < defaultScaleX)
		{
			transform.localScale = new Vector3 (defaultScaleX, defaultScaleY, transform.localScale.z) ;
		}
	}
}
