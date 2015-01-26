using UnityEngine;
using System.Collections;

public class SetRotAtStart : MonoBehaviour {

	public Vector2 rot = new Vector2 ();

	// Use this for initialization
	void Start () {

		GetComponent<vp_FPCamera> ().SetRotation (rot);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
