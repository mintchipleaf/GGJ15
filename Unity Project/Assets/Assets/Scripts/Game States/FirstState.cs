using UnityEngine;
using System.Collections;

public class FirstState : GameStates {

	protected override void Start() {

		base.Start();

		TweeFunctions.TweeEvent += GertFixedTV;

	}

	public void GertFixedTV(string e) {

		if (e != "GertFixTV")
			return;

		completed = true;

	}

}
