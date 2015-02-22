using UnityEngine;
using System.Collections;

public class GameStates : MonoBehaviour {

	public bool completed = false;
	int thisState = 0;

	void Start() {

		thisState = StateManager.Instance.currentState;

	}

	protected virtual void Update() {

		if (completed)
			StateManager.Instance.currentState = thisState + 1;

	}

}
