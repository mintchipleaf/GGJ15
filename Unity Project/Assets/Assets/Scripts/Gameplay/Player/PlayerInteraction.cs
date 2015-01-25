using UnityEngine;
using System.Collections;

public class PlayerInteraction : vp_FPInteractManager {

	CursorMode cursorMode = CursorMode.Auto;
	public Texture2D mainTex;
	public Texture2D grabTex;

	Vector2 hotspot = Vector2.zero;

	Camera thisCamera;

	void Start () {

		Cursor.SetCursor (mainTex, hotspot, cursorMode);
		foreach (Transform child in transform) {
			if (child.name == "FPSCamera")
				thisCamera = child.GetComponent<Camera> ();
		}

	}

	void Update () {

		vp_Interactable interactable;

		Cursor.SetCursor (mainTex, hotspot, cursorMode);

		if (FindInteractable (out interactable)) {
			if (interactable == null)
				return;

			Cursor.SetCursor (grabTex, hotspot, cursorMode);
			if (Input.GetMouseButtonDown (0))
				interactable.TryInteract (m_Player);
		}

	}

	protected override bool FindInteractable (out vp_Interactable interactable) {

		Vector3 outPos = thisCamera.ViewportToWorldPoint (Input.mousePosition);

		interactable = null;

		RaycastHit hit;
		Ray mainRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit mainHit;
		//Camera.main.ScreenPointToRay (Input.mousePosition)
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, MaxInteractDistance, vp_Layer.Mask.BulletBlockers)) {
			// test to see if we hit a collider and if that collider contains a vp_Interactable instance
			if (!m_Interactables.TryGetValue (hit.collider, out interactable))
				m_Interactables.Add (hit.collider, interactable = hit.collider.GetComponent<vp_Interactable> ());
			
			// return if no interactable
			if (interactable == null)
				return false;
			
			// checks our distance, either from this instance's interactDistance, or if it's overridden on the interactable itself. If the hit is within range, carry on
			if (interactable.InteractDistance == 0 && hit.distance >= (m_Player.IsFirstPerson.Get () ? InteractDistance : InteractDistance3rdPerson))
				return false;
			
			// make sure the interact distance isn't higher than the interactables
			if (interactable.InteractDistance > 0 && hit.distance >= interactable.InteractDistance)
				return false;
		} else
			return false;
		
		return true;

	}

}
