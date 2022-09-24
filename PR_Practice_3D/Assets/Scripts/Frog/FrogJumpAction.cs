using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogJumpAction : IFrogAction {

	private float jumpDuration;
	private float jumpHeight;

	private Vector3 position;
	private Vector3 jumpPosition;

	public FrogJumpAction(float jumpDuration, float jumpHeight) {
		this.jumpDuration = jumpDuration;
		this.jumpHeight = jumpHeight;
	}

	public void StartAction(FrogAnimationController frog) {
		frog.SetBool("OnGround", false);
		frog.Play("Jump");
		position = frog.transform.position;
		jumpPosition = position + (Vector3.up * jumpHeight);
	}

	public bool Action(FrogAnimationController frog) {
		float actionTime = frog.GetActionTime() - 0.75f;

		frog.transform.position = Vector3.Lerp(position, jumpPosition, GetValueAtTime(actionTime / jumpDuration));

		return actionTime < jumpDuration;
	}

	public void EndAction(FrogAnimationController frog) {
		frog.SetBool("OnGround", true);
		frog.transform.position = position;
	}

	private float GetValueAtTime(float percentage) {
		return (-4f * (percentage * percentage)) + (4f * percentage);
	}

}
