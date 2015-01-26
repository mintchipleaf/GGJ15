using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	public static StateManager Instance{ get; private set; }

	public GameObject startRoom;
	public GameObject startObjs;
	public GameObject startDoor;
	public List<GameObject> RoomsSpawned = new List<GameObject> ();
	public GameObject RoomPrefab;

	[HideInInspector]
	public DoorScript
		currentDoor;

	public List<GameObject> PossibleStates = new List<GameObject> ();
	public GameObject creditsRoom;
	public bool gameIsOver;
	public int currentState = 0;

	public GameObject StateToSpawn { get { return PossibleStates [currentState]; } }

	public bool alwaysUnlocked = false;

	void Awake () {

		if (Instance == null)
			Instance = this;

	}

	void Start () {

		startObjs.transform.SetParent (startRoom.transform);
		RoomsSpawned.Add (startRoom);
		currentDoor = GetDoor (startRoom);

	}

	public void Loop () {

		currentDoor.OpenDoor ();
		CreateNewRoom ();
		Debug.Log (currentDoor.transform.name);

	}

	public void CreateNewRoom () {

		Cleanup ();
		Vector3 pos = RoomsSpawned [0].transform.position + new Vector3 (0, 0, -30.0f);
		GameObject obj = null;
		if (gameIsOver) {
			obj = Instantiate (creditsRoom, pos + new Vector3 (0, -0.37f, 0), Quaternion.Euler (0, 180, 0)) as GameObject;	
		} else {
			obj = Instantiate (RoomPrefab, pos, Quaternion.identity) as GameObject;
		}
		RoomsSpawned.Add (obj);
		currentDoor = GetDoor (obj);
		GameObject state = Instantiate (StateToSpawn, pos, Quaternion.identity) as GameObject;
		state.transform.SetParent (obj.transform);
		//if (alwaysUnlocked)
		//	currentDoor.DoorUnlocked = true;

	}

	public DoorScript GetDoor (GameObject obj) {

		return obj.GetComponentInChildren<DoorScript> ();

	}

	public void Cleanup () {

		if (RoomsSpawned.Count < 2)
			return;

		GameObject obj = RoomsSpawned [0];

		Destroy (obj.gameObject);

		RoomsSpawned.Remove (obj);

	}

}
