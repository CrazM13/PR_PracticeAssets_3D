using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerInput : ICharacterInput {

	public void CheckCharacterInput(CharacterBase character) {
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (x != 0 || y != 0) {
			Vector3 movement = new Vector3(Time.deltaTime * character.GetSpeed() * x, 0, Time.deltaTime * character.GetSpeed() * y);
			character.MoveToRelative(movement);
		}
	}

}
