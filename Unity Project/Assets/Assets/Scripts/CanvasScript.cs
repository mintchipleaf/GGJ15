using UnityEngine;
using System.Collections;

public class CanvasScript : MonoBehaviour {

	public static CanvasScript Instance { get; private set; }

	void Awake() {

		Instance = this;

	}

}
