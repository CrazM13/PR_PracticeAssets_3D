using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFrogAction {

	public void StartAction(FrogAnimationController frog);

	public bool Action(FrogAnimationController frog);

	public void EndAction(FrogAnimationController frog);

}
