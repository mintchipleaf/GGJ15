using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	public GameObject startRoom;
	public List<GameObject> RoomsSpawned = new List<GameObject>();
	public GameObject RoomPrefab;

	GameObject currentDoor;

	void Start() {

		RoomsSpawned.Add(startRoom);
		Loop();

	}

	public void Loop() {

		CreateNewRoom();
		currentDoor.GetComponent<DoorScript>().OpenDoor();

	}

	public void CreateNewRoom() {

		Vector3 pos = RoomsSpawned [0].transform.position + new Vector3(0, 0, 30.0f);
		GameObject obj = Instantiate(RoomPrefab, pos, Quaternion.identity) as GameObject;
		RoomsSpawned.Add(obj);
		currentDoor = GetDoor(obj);

	}

	GameObject GetDoor(GameObject obj) {

		foreach (Transform t in obj.transform) {

			if (t.name == "Door Trigger") {

				return t.gameObject;

			}

		}

		return null;

	}

	public void Cleanup() {

		if (RoomsSpawned.Count < 1)
			return;

		GameObject obj = RoomsSpawned [0];

		Destroy(obj.gameObject);

		RoomsSpawned.Remove(obj);

	}

}
