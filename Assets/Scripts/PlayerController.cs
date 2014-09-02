using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// Public variables :D
	public float speed;
	public float jumpHeight;
	
	// Private variables
	//		Action/Movement variables
	private bool grounded, jumped, grabbed;
	private bool facingRight, rightBlocked = false, leftBlocked = false;
	private float move;
	private bool hasKey;
	private bool paused;
	//		World variables
	private float phase, shifted;
	private bool onEarth, phaseOn, justShifted, withinDoorFrame, usedKey;
	private GameObject[] EarthObjects = new GameObject[50];
	private GameObject[] KrethysObjects = new GameObject[50];
	private int levelNumber;

	//Sound things
	private GameObject earthMusicPlayer, krethysMusicPlayer, phaseStartPlayer, phaseCancelPlayer, phaseCompletePlayer;
	private GameObject walkSoundPlayer, jumpSoundPlayer, landCrunchSoundPlayer, landVoiceSoundPlayer;
	private GameObject keyPickupSoundPlayer, exitDoorSoundPlayer;
	
	void Start () {
		ObjectRoleCall();
		// Start on Earth
		DisableKrethys();

		grounded = true;
		facingRight = true;
		onEarth = true;
		phaseOn = false;
		justShifted = false;
		hasKey = false;
		usedKey = false;
		withinDoorFrame = false;
		paused = false;

		// Grab BGM
		earthMusicPlayer = GameObject.Find ("EarthBGM");
		krethysMusicPlayer = GameObject.Find ("KrethysBGM");
		// Grab SFX ////////////////////////////////////////////////////
		// Phase SFX
		phaseStartPlayer = GameObject.Find ("pStartSound");
		phaseCancelPlayer = GameObject.Find ("pCancelSound");
		phaseCompletePlayer = GameObject.Find ("pCompleteSound");

		// Key+Door SFX
		keyPickupSoundPlayer = GameObject.Find ("keyPickupSound");
		exitDoorSoundPlayer = GameObject.Find ("exitDoorSound");

		// Player Movement SFX
		walkSoundPlayer = GameObject.Find ("walkSound");
		jumpSoundPlayer = GameObject.Find ("jumpSound");
		landCrunchSoundPlayer = GameObject.Find ("landCrunchSound");
		landVoiceSoundPlayer = GameObject.Find ("landVoiceSound");

		// Mute Krethys
		krethysMusicPlayer.audio.mute = true;
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel (GetCurrentLevelNumber ());
		}
	}
		//Debug.Log((rigidbody2D.position.y - (renderer.bounds.size.y / 2)));
		//Debug.Log("phaseON: "+phaseOn);
		//Debug.Log("justShifted: "+justShifted);
		//Debug.Log("phaseON: "+phaseOn);
		//Debug.Log(grounded);
	
	void FixedUpdate () {
		move = Input.GetAxis ("Horizontal");
		// 
		if(grounded && !phaseOn && withinDoorFrame && hasKey){
			if(Input.GetAxis("Vertical") > 0) {
				keyPickupSoundPlayer.audio.Play();
				exitDoorSoundPlayer.audio.Play();
				usedKey = true;
			}
			if(usedKey && !keyPickupSoundPlayer.audio.isPlaying && !exitDoorSoundPlayer.audio.isPlaying) {
				Application.LoadLevel(GetCurrentLevelNumber() + 1);
			}
		}

		if(Input.GetButton("Shift") && !justShifted) {
			move = 0;

		 	if (!phaseOn && !jumped) {
				phaseOn = true;
				Debug.Log("Phase start...");
				PhaseStart();
			}
		}

		// Shift can cancel only  when phase is on and player did not just shift
		if(Input.GetButtonUp("Shift") && phaseOn){
			Debug.Log("Phase cancelled...");
			PhaseCancel();
			phaseOn = false;
		}

		// release lock on double-shifting to prevent complete de-activation
		if(Input.GetButtonUp("Shift")) {
			justShifted = false;
		}

		// if player presses jump button AND they are grounded, 
		if ((Input.GetAxis("Jump") == 1) && grounded) {
			grounded = false;
			jumped = true;
			// AND if they are phasing and have not just shifted
			if(phaseOn && !justShifted){
				Debug.Log("PHASE SHIFT!!");
				PhaseComplete();
				justShifted = true;
				phaseOn = false;
			}
		}

		if ((rightBlocked && move > 0) || (leftBlocked && move < 0)) move = 0;
		
		rigidbody2D.velocity = new Vector2 (move * speed, rigidbody2D.velocity.y);

		// if the player is in motion, and they are on the ground, play walk sound/animation
		if(rigidbody2D.velocity.x != 0 && grounded) {
			if(!walkSoundPlayer.audio.isPlaying){
				walkSoundPlayer.audio.Play();
			}
		}

		if (jumped) {
			//Debug.Log("I think I have jumped!");
			rigidbody2D.velocity = new Vector2 (move * speed, jumpHeight);
			jumpSoundPlayer.audio.PlayOneShot(jumpSoundPlayer.audio.clip, 1);
			jumped = false;
		}

		if((move > 0) && !facingRight) {
			//Debug.Log("I'm flipping from LEFT to RIGHT");
			Flip();
		} else if((move < 0) && facingRight) {
			//Debug.Log("I'm flipping from RIGHT to LEFT");
			Flip ();
		}
	}

	// Collision Handling ////////////////////////////////////////
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag != "Wall") {
			//Debug.Log("Collision occurred at: "+collision.contacts [0].point.y);
			if (collision.contacts [0].point.y < (rigidbody2D.position.y - 0.45)) {	
				grounded = true;
			}
		}else if (collision.gameObject.tag == "Wall") {
			if (collision.contacts [0].point.x > rigidbody2D.position.x) {
				//Debug.Log("I just ran into a "+collision.gameObject.tag+" on my RIGHT!");
				rightBlocked = true;
			}
			else if (collision.contacts [0].point.x < rigidbody2D.position.x) {
				//Debug.Log("I just ran into a "+collision.gameObject.tag+" on my LEFT!");
				leftBlocked = true;
			}
		}
		if (collision.gameObject.tag == "Floor")
		{
			if (collision.contacts[0].point.y < rigidbody2D.position.y)
			{
				grounded = true;
				landCrunchSoundPlayer.audio.PlayOneShot(landCrunchSoundPlayer.audio.clip, 1);
				landVoiceSoundPlayer.audio.PlayOneShot(landVoiceSoundPlayer.audio.clip, 1);
			}
		}
	}
	
	void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag == "Wall") {
			if (collision.contacts [0].point.x > rigidbody2D.position.x) {
				//Debug.Log("I just ran into a "+collision.gameObject.tag+" on my RIGHT!");
				rightBlocked = true;
			}
			else if (collision.contacts [0].point.x < rigidbody2D.position.x) {
				//Debug.Log("I just ran into a "+collision.gameObject.tag+" on my LEFT!");
				leftBlocked = true;
			}
		}

		if (collision.gameObject.tag == "Floor")
		{
			if (collision.contacts[0].point.y < rigidbody2D.position.y)
			{
				grounded = true;
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.contacts [0].point.y < (rigidbody2D.position.y - 0.45)) {	
			grounded = false;
		}
		
		if (collision.gameObject.tag == "Wall") {
			if (collision.contacts [0].point.x > rigidbody2D.position.x && rightBlocked) {
				rightBlocked = false;
			}
			else if (collision.contacts [0].point.x < rigidbody2D.position.x && leftBlocked) {
				leftBlocked = false;
			}
		}
	}
	// End Collision Handling ////////////////////////////////////////
	//////////////////////////////////////////////////////////////////
	
	/* 	Start of Phase-Shift:
	*	- Activate all game objects
	*	- modify object alphas so that the destination-world becomes apparent,
	*		but the current world remains visible. 
	*	- Play "Phase-Start" sound byte
	*/
	void PhaseStart() {
		if(onEarth) {
			EnableKrethys();
			onEarth = true;
		} else {
			EnableEarth();
			onEarth = false;
		}

		phaseStartPlayer.audio.PlayOneShot(phaseStartPlayer.audio.clip, 1);
	}
	/*	Cancellation of Phase-Shift
	* 	- Reverse the changes made to alphas during Phase Start
	*	- Deactivate the appropriate world's objects (Keep current world's objects)
	*	- Play "Phase-Cancellation" sound byte
	*/
	void PhaseCancel() {
		if(onEarth) {
			DisableKrethys();
			onEarth = true;
		} else {
			DisableEarth();
			onEarth = false;
		}

		phaseCancelPlayer.audio.PlayOneShot(phaseCancelPlayer.audio.clip, 1);
	}
	
	/* 	Completion of Phase-Shift:
	*	- Deactivate appropriate world's objects (Remove current world's objects. Change current world)
	*	- Play "Phase-Completion" sound byte
	*/
	void PhaseComplete() {
		if(onEarth) {
			DisableEarth();
			onEarth = false;
			justShifted = true;
			earthMusicPlayer.audio.mute = true;
			krethysMusicPlayer.audio.mute = false;
		} else {
			DisableKrethys();
			onEarth = true;
			justShifted = true;
			earthMusicPlayer.audio.mute = false;
			krethysMusicPlayer.audio.mute = true;
		}

		phaseCompletePlayer.audio.PlayOneShot(phaseCompletePlayer.audio.clip, 1);
	}

	/*	Roll Call
	 * 	- Tallies up all GameObjects in the scene, and sorts them into 
	 * 	appropriate arrays of Krethys Objects and Earth Objects
	 */
	void ObjectRoleCall() {
		GameObject[] allObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
		int eIndex = 0;
		int kIndex = 0;
		// Categorization of all objects in the scene, into either Earth or Krethys objects, for later simplicity

		for(int i = 0; i < allObjects.Length; i++) {
			//Debug.Log(allObjects[i].name +" : " + allObjects[i].layer);
			if ((allObjects[i].layer > 12 && allObjects[i].layer < 18)
			   || allObjects[i].layer == 20) 
			{
				KrethysObjects[kIndex] = allObjects[i];
				kIndex++;
				//Debug.Log("adding to krethys objects " + allObjects[i].name);
			} 

			else if ((allObjects[i].layer > 7 && allObjects[i].layer < 13)
			        || allObjects[i].layer == 19) 
			{
				EarthObjects[eIndex] = allObjects[i];
				eIndex++;
			}
		}
	}

	void EnableEarth(){
		for(int i = 0; i < EarthObjects.Length; i++) {
			EarthObjects[i].active = true;
			if(!EarthObjects[i+1]) break;
		}
	}
	void DisableEarth() {
		for(int i = 0; i < EarthObjects.Length; i++) {
			EarthObjects[i].gameObject.SetActive(false);
			if(!EarthObjects[i+1]) break;
		}
	}
	void EnableKrethys(){
		for(int i = 0; i < KrethysObjects.Length; i++) {
			KrethysObjects[i].active = true;
			if(!KrethysObjects[i+1]) break;
		}
	}
	void DisableKrethys() {
		for(int i = 0; i < KrethysObjects.Length; i++) {
			KrethysObjects[i].gameObject.SetActive(false);
			if(!KrethysObjects[i+1]) break;
		}
	}
	// Change the direction Player is facing, left/right
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	int GetCurrentLevelNumber() {
		return Application.loadedLevel;
	}

	// for pickups, if we need those
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Key") {
			Destroy(collider.gameObject);
			// Do stuff!
			keyPickupSoundPlayer.audio.PlayOneShot(keyPickupSoundPlayer.audio.clip, 1);
			hasKey = true;
		}
	}

	void OnTriggerStay2D(Collider2D collider){
		if (collider.tag == "ExitDoor") {
			withinDoorFrame = true;
		}
	}
}