using UnityEngine;
using System.Collections;
using Apex;
using Apex.Steering;

public class Mover : MonoBehaviour {

	public Transform target;

	private IMovable _movable;

	bool Leaving = false;

	// Use this for initialization
	private void Awake () {

		_movable = this.As<IMovable> ();
	
	}
	
	// Update is called once per frame
	public void MoveToDestination () {

		_movable.MoveTo (target.position, true);
	
	}

	public void ExitRoom () {
		if (transform.name == "Sheila") {
			GetComponent<DialogueEvent> ().interactPassage = "Sheila sad";
			return;
		}
		if (StateManager.Instance.RoomsSpawned.Count > 1)
			target = StateManager.Instance.GetDoor (StateManager.Instance.RoomsSpawned [1]).transform;
		else
			target = StateManager.Instance.GetDoor (StateManager.Instance.RoomsSpawned [0]).transform;


		Leaving = true;

		MoveToDestination ();

	}

	void Update () {

		if (Leaving) {

			if (Vector3.Distance (target.position, transform.position) <= 3.0f)
				Destroy (this.gameObject);

		}

	}
}