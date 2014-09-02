using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	
	private bool isActive = false;
	private bool haveObject = false;
	private GameObject[] buttonObstacles;

	// Use this for initialization
	void Start () {
		buttonObstacles = GameObject.FindGameObjectsWithTag ("ButtonControlledObstacle");
		if(buttonObstacles.Length == 0) haveObject = false;
		else haveObject = true;
	}
	
	// Update is called once per frame
	void Update () 
	{ 
	if (isActive && haveObject)
		{
			//Debug.Log ("turning off");
			for(int i = 0; i < buttonObstacles.Length; i++) {
				buttonObstacles[i].SetActive (false);
			}
		}
		else if (!isActive && haveObject)
		{
			for(int i = 0; i < buttonObstacles.Length; i++) {
				buttonObstacles[i].SetActive (true);
			}	
		}
		// Breakfast! :D BRB
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.attachedRigidbody.tag == "Player"
		    || collision.collider.attachedRigidbody.tag == "Box") 
		{
			isActive = true;
			Debug.Log ("touching player");
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.attachedRigidbody.tag == "Player"
		    || collision.collider.attachedRigidbody.tag == "Box") 
		{
			isActive = false;
			Debug.Log ("not touching player");
		}
	}
}
