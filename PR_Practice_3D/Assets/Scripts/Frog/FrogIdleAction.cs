using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogIdleAction : IFrogAction {

	private float duration;

	public FrogIdleAction(float durationOfIdle) {
		this.duration = durationOfIdle;
	}

	public bool Action(FrogAnimationController frog) {
		return frog.GetActionTime() < duration;
	}

	public void EndAction(FrogAnimationController frog) { /*MT*/ }

	public void StartAction(FrogAnimationController frog) { /*MT*/ }
}
