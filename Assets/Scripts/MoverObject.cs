using UnityEngine;
using System.Collections;

public class MoverObject : MonoBehaviour 
{
	public PositionMoveTos[] positions;
	private int moveToHold;

	private int currentPos;

	private float timeTilAction;

	private bool moving = false;
	
	void Start () 
	{
		currentPos = 0;
		transform.position = positions[0].Position;
		moveToHold = positions[0].BeatsPerWait; 
	}

	void Update () 
	{
		if(Overlord.Instance.TO.Beat) CallBeat ();
	}

	public void CallBeat()
	{
		moveToHold--;
		if(moveToHold == 0)
		{
			if(moving)
			{
				transform.position = positions[currentPos].Position;
				rigidbody2D.velocity = Vector2.zero;
				moveToHold = positions[currentPos].BeatsPerWait;
				moving = false;
			}

			if(!moving && moveToHold == 0)
			{
				if(currentPos == positions.Length - 1)
				{
					currentPos = 0;
				}
				else currentPos ++;

				moving = true;

				rigidbody2D.velocity = (positions[currentPos].Position - (Vector2)transform.position)/(positions[currentPos].BeatsPerMove * Overlord.Instance.TO.beatLength);
				moveToHold = positions[currentPos].BeatsPerMove;
			}

		}
	}
}

[System.Serializable]
public class PositionMoveTos
{
	[SerializeField]
	private GameObject obj;
	public int BeatsPerMove;
	public int BeatsPerWait;


	public Vector2 Position
	{
		get{return obj.transform.position;}
	}
}