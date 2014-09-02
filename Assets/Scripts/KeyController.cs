using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {
	public float height = 0.0f;
	public float bobSpeed;
	private Vector2 temp;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		temp = new Vector2(transform.position.x, 1.0f);
		temp.y = (temp.y * Mathf.Sin(Time.timeSinceLevelLoad) * bobSpeed) + height;
		transform.position = temp;
	}
}