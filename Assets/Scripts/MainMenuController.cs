using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	private GameObject[] EarthObjects = new GameObject[5];
	private GameObject[] KrethysObjects = new GameObject[5];
	private bool shift, justShifted;
	private bool onEarth, phaseOn;
	private GameObject phaseStartPlayer, phaseCancelPlayer, phaseCompletePlayer;
	private GameObject titleText1, titleText2, titleText3;
	// Use this for initialization
	void Start () {
		ObjectRoleCall();
		onEarth = true;
		shift = false;
		justShifted = false;
		DisableKrethys();

		phaseStartPlayer = GameObject.Find ("pStartSound");
		phaseCancelPlayer = GameObject.Find ("pCancelSound");
		phaseCompletePlayer = GameObject.Find ("pCompleteSound");

		titleText1 = GameObject.Find("Title_Text1");
		titleText2 = GameObject.Find("Title_Text2");
		titleText3 = GameObject.Find("Title_Text3");

		titleText2.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// if player is holding down shift, and they have not just phased
		if(Input.GetButton("Shift")) {
			// if phase is not on, and the player is not shifting
			if (!phaseOn && !shift) {
				// set phaseOn->true, and call PhaseStart();
				phaseOn = true;
				Debug.Log("Phase start...");
				PhaseStart();
			}
		}
		
		// Phase can cancel only when phase is on and player did not just shift
		if(Input.GetButtonUp("Shift") && phaseOn){
			Debug.Log("Phase cancelled...");
			PhaseCancel();
			phaseOn = false;
		}
		
		// if player presses jump button AND they are grounded, 
		if (Input.GetButtonDown("Enter")) {
			Application.LoadLevel(GetCurrentLevelNumber()+1);
		}
	}
	/* 	Start of Phase-Shift:
	*	- Activate all game objects
	*	- modify object alphas so that the destination-world becomes apparent,
	*		but the current world remains visible. 
	*	- Play "Phase-Start" sound byte
	*/
	void PhaseStart() {
		if(onEarth) {
			//SetTextActive(false, true, false);
			EnableKrethys();
			onEarth = true;
		} else {
			//SetTextActive(false, false, true);
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
			//SetTextActive(true, false, false);
			DisableKrethys();
			onEarth = true;
		} else {
			//SetTextActive(false, false, true);
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

		} else {
			DisableKrethys();
			onEarth = true;
			justShifted = true;
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

	int GetCurrentLevelNumber() {
		return Application.loadedLevel;
	}
}
