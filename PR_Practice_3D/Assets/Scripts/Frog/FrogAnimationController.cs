using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimationController : MonoBehaviour {

	[SerializeField] private new Animator animation;

	private List<IFrogAction> actions = new List<IFrogAction>();
	private int currentAction = -1;

	float timeInCurrentAction = 0;

	void Start() {
		actions.Add(new FrogIdleAction(3));
		actions.Add(new FrogIdleAction(1));
		actions.Add(new FrogIdleAction(4.5f));
		actions.Add(new FrogJumpAction(2, 4));

		currentAction = 0;
		actions[currentAction].StartAction(this);
	}

	void Update() {
		timeInCurrentAction += Time.deltaTime;

		if (actions.Count > 0) PlayAction();
	}

	private void PlayAction() {
		if (!actions[currentAction].Action(this)) {
			ChangeAction();
		}
	}

	public void Play(string trigger) {
		animation.SetTrigger(trigger);
	}

	public void SetBool(string boolean, bool value) {
		animation.SetBool(boolean, value);
	}

	public float GetActionTime() => timeInCurrentAction;

	private void ChangeAction() {
		timeInCurrentAction = 0;

		actions[currentAction].EndAction(this);
		currentAction = (currentAction + Random.Range(1, actions.Count)) % actions.Count;
		actions[currentAction].StartAction(this);
	}

}
