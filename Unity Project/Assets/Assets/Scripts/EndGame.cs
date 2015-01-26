using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public GameObject credits;
	public Camera creditsCamera;

	// Use this for initialization
	void Start () {
		creditsCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider entity){
		//if(entity.name == "Player"){}
		GameObject creditsCanvas = (GameObject)Instantiate (credits, Vector3.zero, Quaternion.identity);
		creditsCamera.enabled = true;
	}
}
