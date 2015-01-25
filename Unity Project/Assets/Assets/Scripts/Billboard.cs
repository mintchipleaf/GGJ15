using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	//Quaternion qt;

	// Update is called once per frame
	void Update () {
		//qt = transform.rotation;
		transform.LookAt(Camera.main.transform.position, Vector3.up);
		//qt.x = 0;
		//transform.rotation = qt;
	}
}
