using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayer : CharacterBase {

	protected override void OnInitialize() {
		this.SetCharacterInput(new CharacterPlayerInput());
	}

}
