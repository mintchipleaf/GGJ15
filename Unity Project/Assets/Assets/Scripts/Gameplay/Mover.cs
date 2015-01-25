using UnityEngine;
using System.Collections;
using Apex;
using Apex.Steering;

public class Mover : MonoBehaviour {

	public Transform target;

	private IMovable _movable;

	// Use this for initialization
	private void Awake () {

		_movable = this.As<IMovable> ();
	
	}
	
	// Update is called once per frame
	public void MoveToDestination () {

		_movable.MoveTo (target.position, true);
	
	}
}