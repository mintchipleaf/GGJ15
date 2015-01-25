using UnityEngine;
using System.Collections;

public class DialogueEvent : vp_Interactable {
	public string passageName;
	public string interactPassage;

	private float distanceToPlayer;
	private float brightness;
	private Canvas canvas;

	public void Start () {
		canvas = GetComponentInChildren<Canvas> ();
		DialogueDisplay.Instance.Display (passageName, canvas);
	}

	public void Update () {
		distanceToPlayer = Vector3.Distance (transform.position, Player.instance.transform.position);
		//Debug.Log(distanceToPlayer)
		brightness = 1 - distanceToPlayer / 10;
		if (distanceToPlayer >= 10)
			brightness = 0;
		if (distanceToPlayer <= 2)
			brightness = 1;
		canvas.GetComponent<CanvasGroup> ().alpha = brightness;
	}

	public override bool TryInteract (vp_FPPlayerEventHandler player) {

		DialogueDisplay.Instance.Cleanup (canvas.transform);

		TweeFunctions.Instance.AddCallback (TalkToGirt);
		TweeFunctions.Instance.AddCallback (TalkToSheila);

		DialogueDisplay.Instance.Display (interactPassage, canvas);
		return base.TryInteract (player);
	}

	public void TalkToGirt (string e) {

		if (e != "TestGirt")
			return;

		GetComponent<Mover> ().MoveToDestination ();
		TweeFunctions.Instance.RemoveCallback (TalkToGirt);

	}

	public void TalkToSheila (string e) {

		if (e != "TalkToSheila")
			return;

		GetComponent<Mover> ().MoveToDestination ();
		TweeFunctions.Instance.RemoveCallback (TalkToSheila);

	}

	/*public override bool TryInteract (vp_FPPlayerEventHandler player)
	{
		return base.TryInteract (player);
	}*/
}
