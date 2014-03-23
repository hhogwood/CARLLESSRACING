using UnityEngine;
using System.Collections;

public class MoverObject : MonoBehaviour 
{
	public GameObject[] moveToPositions;
	public float[] moveToTimes;
	public int[] moveToWaits;
	private int moveToHold;

	private int currentPos;

	private float timeTilAction;

	private bool moving = false;
	
	void Start () 
	{
		currentPos = 0;
		transform.position = moveToPositions[0].transform.position;
		moveToHold = moveToWaits[0]; 
	}

	void Update () 
	{
		if(moving)
		{
			if((transform.position - moveToPositions[currentPos].transform.position).magnitude < 0.1f)
			{
				transform.position = moveToPositions[currentPos].transform.position;
				rigidbody2D.velocity = Vector2.zero;
				moveToHold = moveToWaits[currentPos];
				moving = false;
			}
		}

		CallBeat ();
	}

	public void CallBeat()
	{
		moveToHold--;
		if(moveToHold == 0)
		{
			if(currentPos == moveToPositions.Length - 1)
			{
				currentPos = 0;
			}
			else currentPos ++;

			moving = true;


			rigidbody2D.velocity = ( moveToPositions[currentPos].transform.position - transform.position)/moveToTimes[currentPos];
		}
	}
}