using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static Player instance {get;private set;}

	// Use this for initialization
	void Awake () {
		if(instance == null)
			instance = this;
	}
}
