using UnityEngine;
using System.Collections;

public class DialogueEvent : vp_Interactable
{
	public string passageName;

	public void Start(){
		DialogueDisplay.Instance.Display (passageName, GetComponentInChildren<Canvas>().transform);
	}

	/*public override bool TryInteract (vp_FPPlayerEventHandler player)
	{
		return base.TryInteract (player);
	}*/

}
