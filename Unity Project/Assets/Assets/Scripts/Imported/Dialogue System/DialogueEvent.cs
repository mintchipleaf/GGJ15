using UnityEngine;
using System.Collections;

public class DialogueEvent : vp_Interactable
{

		public override bool TryInteract (vp_FPPlayerEventHandler player)
		{


				DialogueDisplay.Instance.Display ("Begin");
				return base.TryInteract (player);
		}

}
