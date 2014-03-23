using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour 
{
	private int numberOfLives;
	public bool dead = false;

	public GameObject lifeCountPrefab;
	private float lifeIconDifference = 0.27f;

	private List<GameObject> lifeIcons = new List<GameObject>();
	private Vector3 uiPos;
	private Color uiColor;


	public void InitializePlayer(int lifeCount, Vector2 uiLocation, Color lifeColor)
	{
		numberOfLives = lifeCount;
		GetComponent<Moving>().myPlayer = this;

		uiPos = uiLocation;
		uiColor = lifeColor;

		/*Vector3 temp;
		for(int i = 0; i < numberOfLives; i++)
		{
			lifeIcons.Add((GameObject)GameObject.Instantiate(lifeCountPrefab) as GameObject);
			lifeIcons[i].GetComponent<SpriteRenderer>().color = lifeColor;

			lifeIcons[i].transform.parent = Camera.main.transform;

			temp = uiLocation;
			temp.z = 1;
			lifeIcons[i].transform.localPosition = temp;


			uiLocation.x += lifeIconDifference;
		}*/
	}

	public void AddScore()
	{
		lifeIcons.Add((GameObject)GameObject.Instantiate(lifeCountPrefab) as GameObject);
		lifeIcons[lifeIcons.Count - 1].GetComponent<SpriteRenderer>().color = uiColor;
		
		lifeIcons[lifeIcons.Count - 1].transform.parent = Camera.main.transform;

		uiPos.z = 1;
		lifeIcons[lifeIcons.Count - 1].transform.localPosition = uiPos;
		
		
		uiPos.x += lifeIconDifference;
	}

	public void PlayerDeath()
	{
		/*if(numberOfLives > 1)
		{
			numberOfLives--;
			GameObject.Destroy (lifeIcons[lifeIcons.Count - 1]);
			lifeIcons.RemoveAt (lifeIcons.Count - 1);
		}
		else
		{
			dead = true;
			for(int i = 0; i < lifeIcons.Count; i++)
			{
				GameObject.Destroy (lifeIcons[lifeIcons.Count - 1]);
				lifeIcons.RemoveAt (lifeIcons.Count - 1);
			}
		}*/
	}
}
