using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	public static StateManager Instance{ get; private set; }

	public GameObject startRoom;
	public List<GameObject> RoomsSpawned = new List<GameObject> ();
	public GameObject RoomPrefab;

	DoorScript currentDoor;

	public List<GameObject> PossibleStates = new List<GameObject> ();
	public int currentState = 0;

	public GameObject StateToSpawn { get { return PossibleStates [currentState]; } }

	void Awake () {

		if (Instance == null)
			Instance = this;

	}

	void Start () {

		RoomsSpawned.Add (startRoom);

	}

	public void Loop () {

		CreateNewRoom ();
		currentDoor.GetComponent<DoorScript> ().OpenDoor ();

	}

	public void CreateNewRoom () {

		Cleanup ();
		Vector3 pos = RoomsSpawned [0].transform.position + new Vector3 (0, 0, -30.0f);
		GameObject obj = Instantiate (RoomPrefab, pos, Quaternion.identity) as GameObject;
		RoomsSpawned.Add (obj);
		currentDoor = GetDoor (obj);
		GameObject state = Instantiate (StateToSpawn, pos, Quaternion.identity) as GameObject;
		state.transform.SetParent (obj.transform);

	}

	DoorScript GetDoor (GameObject obj) {

		return obj.GetComponentInChildren<DoorScript> ();

	}

	public void Cleanup () {

		if (RoomsSpawned.Count < 2)
			return;

		GameObject obj = RoomsSpawned [0];

		Destroy (obj.gameObject);

		RoomsSpawned.Remove (obj);

	}

	IEnumerator Cycle () {

		while (true) {
			yield return new WaitForSeconds (10.0f);
			Loop ();
		}

	}

}
