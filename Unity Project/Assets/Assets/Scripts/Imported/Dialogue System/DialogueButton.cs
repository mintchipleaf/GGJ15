using UnityEngine;
using System.Collections;

public class DialogueButton : MonoBehaviour {

	public string linkTo;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnMouseDown () {

		if (linkTo != "") {
			DialogueDisplay.Instance.Display (linkTo);
		}
		if (linkTo == "" || linkTo == null) {
			Debug.LogError ("There is no passage associated with this link! Please figure it out.");
		}
	
	}
}
