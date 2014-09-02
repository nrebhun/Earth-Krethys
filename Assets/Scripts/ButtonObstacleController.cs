using UnityEngine;
using System.Collections;

public class ButtonObstacleController : MonoBehaviour 
{

	bool buttonActive = false;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		/*
		GameObject button = GameObject.Find ("Button");
		buttonActive = button.GetComponent <ButtonController>().isActive;

		Debug.Log ("buttonActive is " + buttonActive);

		if (buttonActive) //(button.GetComponent (ButtonController.isTouchingPlayer))
		{
			//Debug.Log ("yes");
			collider2D.gameObject(SpriteRenderer).enabled = false;
			collider2D.enabled = false;
		}
		else 
		{
			//Debug.Log ("no");
			collider2D.gameObject.SetActive(true);
			collider2D.enabled = true;
		}*/

		//Debug.Log ("touching is " + touching);
	}
	
}

