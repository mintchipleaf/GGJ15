using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public static TimeManager Instance { get; private set; }

	public float GameTime = 0.0f;
	public bool TimerActive = false;

	public float GameLength = 180.0f;

	public Text thisText;

	void Awake () {
		if (Instance == null)
			Instance = this;
	}

	void Start () {

		RestartTime ();

	}

	public void RestartTime () {

		GameTime = 0.0f;
		TimerActive = true;

	}

	void Update () {

		if (TimerActive)
			GameTime += Time.deltaTime;

		if (GameTime >= GameLength)
			TimerActive = true;

		thisText.text = "Time: " + GameTime;

	}

}
