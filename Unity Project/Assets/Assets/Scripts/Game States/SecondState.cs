using UnityEngine;
using System.Collections;

public class SecondState : GameStates {

	protected override void Start() {

		base.Start();

		TweeFunctions.TweeEvent += GertFinishSecond;

	}

	void GertFinishSecond(string e) {

		if (e != "GertFinish2")
			return;

		completed = true;

	}

}
