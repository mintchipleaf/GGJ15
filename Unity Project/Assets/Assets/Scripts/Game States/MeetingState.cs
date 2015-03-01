using UnityEngine;
using System.Collections;

public class MeetingState : GameStates {

	public int MeetingCount = 0;

	protected override void Update () {

		if (completed == false && MeetingCount >= 2) {
			//StateManager.Instance.currentDoor.DoorUnlocked = true;
			completed = true;
		}

		base.Update();

	}

}
