using UnityEngine;
using System.Collections;

public class FirstState : GameStates {

	void Start() {

		TweeFunctions.TweeEvent += GertFixedTV;

	}

	public void GertFixedTV(string e) {

		if (e != "GertFixTV")
			return;

		completed = true;

	}

}
