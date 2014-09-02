using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {

	bool touchingPlayer = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (rigidbody2D.velocity.x > 0 && touchingPlayer)
		{
			Debug.Log ("pushing box");
		}
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.attachedRigidbody)
		{
			if (collision.collider.attachedRigidbody.tag == "Player")
			{
				//Debug.Log ("touching player");
				touchingPlayer = true;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.attachedRigidbody)
		{
			if (collision.collider.attachedRigidbody.tag == "Player")
			{
				//Debug.Log ("not touching player");
				touchingPlayer = false;
			}
		}
	}
}
