using UnityEngine;
using System.Collections;
using InControl;

public class Menu : MonoBehaviour 
{
	public GameObject[] InspectorSelects;
	private SelectableItem[] selectables;

	private int currentSelected = 0;

	private bool upJustPressed = false;
	private bool downJustPressed = false;
	private float inputCD = 0.2f;
	private float inputTime;

	void Start()
	{
		selectables = new SelectableItem[InspectorSelects.Length];
		selectables[0] = new Start(InspectorSelects[0]);
		selectables[1] = new Quit(InspectorSelects[1]);
			
		selectables[currentSelected].Hover();
	}

	// Update is called once per frame
	void Update () 
	{
		if(InputManager.ActiveDevice.Action1)
		{
			selectables[currentSelected].Select ();
		}

		if(InputUp())
		{
			Increment ();
		}
		else if(InputDown())
		{
			Decrement();
		}
	}

	private void Increment()
	{
		selectables[currentSelected].UnHover();
		if(currentSelected >= selectables.Length - 1)
		{

			currentSelected = 0;

		}
		else currentSelected++;
		selectables[currentSelected].Hover();
	}

	private void Decrement()
	{
		selectables[currentSelected].UnHover();
		if(currentSelected <= 0)
		{
			currentSelected = selectables.Length - 1;
		}
		else currentSelected--;
		selectables[currentSelected].Hover();
	}


	private bool InputUp()
    {
		if(!upJustPressed && (InputManager.ActiveDevice.DPadUp || InputManager.ActiveDevice.LeftStickY > 0.5f))
		{
			upJustPressed = true;
			inputTime = Time.time + inputCD;
			return true;
		}
		else
		{

			if(upJustPressed && (!InputManager.ActiveDevice.DPadUp && InputManager.ActiveDevice.LeftStickY <= 0.5f))
			{
				upJustPressed = false;
			}
			return false;
		}
	}

	private bool InputDown()
	{

		if(!downJustPressed && (InputManager.ActiveDevice.DPadDown || InputManager.ActiveDevice.LeftStickY < -0.5f))
		{
			downJustPressed = true;
			inputTime = Time.time + inputCD;
			return true;
		}
		else
		{
			if(downJustPressed && (!InputManager.ActiveDevice.DPadDown && InputManager.ActiveDevice.LeftStickY >= -0.5f))
			{
				downJustPressed = false;
			}
			return false;
		}
	}
}

public abstract class SelectableItem
{
	private GameObject item;
	private SpriteRenderer sprite;

	public SelectableItem(GameObject _obj)
	{
		item = _obj;
		sprite = item.GetComponent<SpriteRenderer>();
	}

	public void Hover()
	{
		sprite.color = Color.blue;
	}

	public void UnHover()
	{
		sprite.color = Color.white;
	}

	public abstract void Select();
}

public class Start : SelectableItem
{
	public Start(GameObject _obj) : base(_obj)
	{

	}

	public override void Select ()
	{
		Application.LoadLevel("Scene");
	}
}

public class Quit : SelectableItem
{
	public Quit(GameObject _obj) : base(_obj)
	{
		
	}
	
	public override void Select ()
	{
		Application.Quit();
	}
}