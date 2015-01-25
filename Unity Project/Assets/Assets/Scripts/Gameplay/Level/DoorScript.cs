using UnityEngine;
using System.Collections;

public class DoorScript : vp_Interactable {

	public bool DoorUnlocked = false;

	Renderer[] renderers;
	BoxCollider[] colliders;
	public bool closed = true;
	void Awake () {

		renderers = GetComponentsInChildren<Renderer> ();
		colliders = GetComponentsInChildren<BoxCollider> ();

	}

	public override bool TryInteract (vp_FPPlayerEventHandler player) {
		//if (DoorUnlocked == false)
		//return false;

		//OpenDoor ();

		return base.TryInteract (player);
	}

	public void OpenDoor () {

		//StateManager.Instance.Loop ();

		foreach (Renderer r in renderers) {
			r.enabled = false;
		}
		foreach (BoxCollider c in colliders) {
			c.enabled = false;
		}
		//DialogueDisplay.Instance.Cleanup ();
		//StateManager.Instance.alwaysUnlocked = true;

		closed = false;
		//DoorUnlocked = false;

	}

	public void CloseDoor () {

		foreach (Renderer r in renderers) {
			r.enabled = true;
		}
		foreach (BoxCollider c in colliders) {
			c.enabled = true;
		}

		DoorUnlocked = false;

		closed = true;

	}

}
