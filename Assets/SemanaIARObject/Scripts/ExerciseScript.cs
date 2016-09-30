using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Vuforia;

public class ExerciseScript : MonoBehaviour, ITrackableEventHandler {
	public int objectsMax; 
	public int sidesMax;
	public GameObject buttonNext,buttonError;
	private ArrayList objectsArray = new ArrayList();
	private ArrayList objectsNamesArray = new ArrayList();
	private TrackableBehaviour mTrackableBehaviour;
	private string currentObjectName="",currentTrackedObjectName="";
	private Transform objectImageDisplay = null;

	//Better load from db. 
	public GameObject currentObject, object1, object2, object3, object4, object5,object6 ,object7 ,object8;

	// Use this for initialization
	void Start () {
		Debug.Log ("Init ExerciseScript");

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

		//objectsArray.Add (object1);
		//objectsNamesArray.Add ("iso6");
		//objectsArray.Add (object2);
		//objectsNamesArray.Add ("tornillo");//falta
		objectsArray.Add (object3);
		objectsNamesArray.Add ("iso3");
		objectsArray.Add (object4);
		objectsNamesArray.Add ("iso2");
		objectsArray.Add (object5);
		objectsNamesArray.Add ("iso1");
		objectsArray.Add (object6);
		objectsNamesArray.Add ("iso8");
		//objectsArray.Add (object7);
		//objectsNamesArray.Add ("iso9");
		objectsArray.Add (object8);
		objectsNamesArray.Add ("iso7");

		displayNewObject ();

	}

	public void displayNewObject(){
		currentObject = null;
		currentObjectName = "";
		currentTrackedObjectName = "";
	
		buttonError.gameObject.SetActive (false);
		buttonNext.gameObject.SetActive (false);

		if (objectImageDisplay != null) {
			objectImageDisplay.gameObject.SetActive (false);
		}

		//Get random object. 
		int auxRandom = getRandomObject ();
		currentObject = (GameObject) objectsArray[auxRandom];
		currentObjectName = (string) objectsNamesArray [auxRandom];

		//Get random side to display. 
		objectImageDisplay = currentObject.gameObject.transform.GetChild(getRandomSide ());
		objectImageDisplay.gameObject.SetActive (true);
	}
		
	// Update is called once per frame
	void Update () {
		// Get the Vuforia StateManager
		StateManager sm = TrackerManager.Instance.GetStateManager ();
		// Currently 'active' trackables 
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();
		// Iterate through the list of active trackables
		//Debug.Log ("List of trackables currently active (tracked): ");
		foreach (TrackableBehaviour tb in activeTrackables) {
			//Debug.Log("Trackable--------------------: " + tb.TrackableName);
			//Assing object tracked name to current. 
			currentTrackedObjectName = tb.TrackableName;
		}

		if (currentObjectName.Equals (currentTrackedObjectName)) {
			//Debug.Log ("OnTrackableStateChanged Sucess----------------------------------");
			buttonNext.gameObject.SetActive (true);
			buttonError.gameObject.SetActive (false);
		} else {
			if (!currentTrackedObjectName.Equals ("")) {
				//Debug.Log ("OnTrackableStateChanged Error--------------------");
				buttonError.gameObject.SetActive (true);
				buttonNext.gameObject.SetActive (false);
			} else {
				buttonError.gameObject.SetActive (false);
			}
		}
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		// If we need it here it is. :).
	} 

	private int getRandomObject(){
		int r = Random.Range (0, objectsMax-1);
		return r;
	}

	private int getRandomSide(){
		int r = Random.Range (0, sidesMax-1);
		return r;
	}

}
