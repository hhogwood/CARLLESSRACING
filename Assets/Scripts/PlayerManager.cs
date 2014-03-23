using UnityEngine;
using System.Collections;
using InControl;

public class PlayerManager : MonoBehaviour 
{
	private int numberOfPlayers;
	private int numberAlive;

	public GameObject baseCharacterObject;
	private Player[] players;
	public Sprite[] CharacterColours;
	public Color[] UIColors;
	public GameObject[] spawnLocations;

	public float leftKill;
	public float rightKill;
	public float topKill;
	public float botKill;

	private Vector2 UIOnePos = new Vector2(-15, -8);
	private float UImoveAmount = 1f;

	void Start()
	{
		GameObject tempObj;
		numberOfPlayers = InputManager.Devices.Count;
		players = new Player[numberOfPlayers];
		if(numberOfPlayers > 8) numberOfPlayers = 8;
		for(int i = 0; i < numberOfPlayers; i++)
		{
			tempObj = (GameObject)GameObject.Instantiate(baseCharacterObject) as GameObject;
			tempObj.transform.position = spawnLocations[i].transform.position;
			tempObj.GetComponent<Moving>().playerController = InputManager.Devices[i];
			tempObj.GetComponent<Moving>().manager = this;
			players[i] = tempObj.GetComponent<Player>();
			players[i].InitializePlayer(3, UIOnePos, UIColors[i]);
			UIOnePos.x += UImoveAmount;
			tempObj.GetComponent<SpriteRenderer>().sprite = CharacterColours[i];
		}
	}

	void Update()
	{
		if(numberOfPlayers > 1)
		{
			numberAlive = 0;
			for(int i = 0; i < numberOfPlayers; i++)
			{
				if(!players[i].dead) numberAlive++;
			}

			if(numberAlive <= 1)
			{

			}
		}

		if(InputManager.ActiveDevice.Action2)
		{
			Application.LoadLevel("Scene");
		}
	}

}
