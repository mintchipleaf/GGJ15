using UnityEngine;
using System.Collections;

public class MeetingState : GameStates {

	public int MeetingCount = 0;

	void Update () {

		if (completed == false && MeetingCount >= 2) {
			StateManager.Instance.Loop ();
			completed = true;
		}

	}

}
