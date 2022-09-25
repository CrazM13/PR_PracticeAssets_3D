using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Landscape", menuName = "3D Tiles/Landscape", order = 0)]
public class TileLandscape : ScriptableObject {
	
	[System.Serializable]
	public class TileLandscapeLayer {
		[SerializeField] public RuleTile tile;
		[SerializeField] public int layers;
	}

	public TileLandscapeLayer[] layers;

}
