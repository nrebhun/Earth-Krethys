using UnityEngine;
using System.Collections;

public class PressurePlateController : MonoBehaviour {

	private GameObject platform, wall_1, wall_2;

	// Use this for initialization
	void Start () {
		platform = GameObject.Find ("Krethys Platform Size 2 removable");
		wall_1 = GameObject.Find ("krethys wall 1");
		wall_2 = GameObject.Find ("krethys wall 2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.collider.tag == "Box")
		{
			platform.SetActive (false);
			wall_1.SetActive (false);
			wall_2.SetActive (false);
		}
	}

	/*void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag == "Box")
		{
			platform.SetActive (true);
			wall_1.SetActive (true);
			wall_2.SetActive (true);
		}
	}*/
}
