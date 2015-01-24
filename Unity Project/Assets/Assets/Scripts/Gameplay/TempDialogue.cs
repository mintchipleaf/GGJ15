using UnityEngine;
using System.Collections;

public class TempDialogue : DialogueEvent {

	public string passage = "";
	bool talkedTo = false;

	public override bool TryInteract (vp_FPPlayerEventHandler player) {

		if (talkedTo)
			return false;

		TweeFunctions.Instance.AddCallback (TalkToSheila);
		TweeFunctions.Instance.AddCallback (TalkToGert);
		DialogueDisplay.Instance.Display (passage);
		return base.TryInteract (player);
	}

	public void TalkToSheila (string e) {

		if (e != "TalkToSheila")
			return;

		GetComponent<Mover> ().MoveToDestination ();
		GetComponentInParent<MeetingState> ().MeetingCount++;

	}

	public void TalkToGert (string e) {

		if (e != "TestGert")
			return;

		GetComponent<Mover> ().MoveToDestination ();
		GetComponentInParent<MeetingState> ().MeetingCount++;

	}

}
