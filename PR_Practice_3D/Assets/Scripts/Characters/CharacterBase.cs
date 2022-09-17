using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterBase : MonoBehaviour {

	[SerializeField, Min(0)] private float movementSpeed = 1;
	[SerializeField] private ICharacterInput characterInput;

	private Vector3? target;

	void Awake() {
		OnInitialize();
	}

	void Update() {
		if (target.HasValue) UpdateMovement();

		if (characterInput != null) characterInput.CheckCharacterInput(this);
	}

	private void UpdateMovement() {
		Vector3 targetMovement = Vector3.MoveTowards(transform.position, target.Value, GetSpeed());

		if (targetMovement == target) {
			transform.position = target.Value;
			target = null;
		} else {
			transform.position = targetMovement;
		}
	}

	public void MoveTo(Vector3 targetPosition) {
		NavMesh.SamplePosition(targetPosition, out NavMeshHit sample, 1, -1);
		if (sample.hit) {
			target = sample.position;
		}
	}

	public void MoveToRelative(Vector3 targetPosition) {
		MoveTo(transform.position + targetPosition);
	}

	public void SetCharacterInput(ICharacterInput input) {
		this.characterInput = input;
	}

	protected abstract void OnInitialize();

	public float GetSpeed() {
		return movementSpeed;
	}

}
