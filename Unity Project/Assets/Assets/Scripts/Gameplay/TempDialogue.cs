using UnityEngine;
using System.Collections;

public class TempDialogue : DialogueEvent {

	public string passage = "";


	public override bool TryInteract (vp_FPPlayerEventHandler player) {

		TweeFunctions.Instance.AddCallback (TalkToSheila);
		TweeFunctions.Instance.AddCallback (TalkToGert);
		DialogueDisplay.Instance.Display (passage);
		return base.TryInteract (player);
	}

	public void TalkToSheila (string e) {

		if (e != "TalkToSheila")
			return;

	}

	public void TalkToGert (string e) {

		if (e != "TestGert")
			return;

	}

}
