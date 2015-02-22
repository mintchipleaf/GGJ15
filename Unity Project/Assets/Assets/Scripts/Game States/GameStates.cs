using UnityEngine;
using System.Collections;

public class GameStates : MonoBehaviour {

	public bool completed = false;
	int thisState = 0;

	protected virtual void Start() {

		thisState = StateManager.Instance.currentState;

	}

	protected virtual void Update() {

		if (completed && StateManager.Instance.currentState == thisState) {
			StateManager.Instance.currentState = thisState + 1;
		}

	}

}
