using UnityEngine;
using System.Collections;

public class DoorScript : vp_Interactable {

	public void OpenDoor() {

		this.GetComponent<Renderer>().enabled = false;

	}

}
