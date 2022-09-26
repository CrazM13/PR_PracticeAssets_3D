using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Landscape", menuName = "3D Tiles/Landscape", order = 0)]
public class TileLandscape : ScriptableObject {
	
	[System.Serializable]
	public class TileLandscapeLayer {
		[SerializeField] public TileBase tile;
		[SerializeField] public int layers;
	}

	public TileLandscapeLayer[] layers;

	public TileBase GetTileByY(int y) {
		int layerY = 0;
		for (int i = 0; i < y; i++) {
			if (layers[layerY].layers <= i) {
				if (layerY < layers.Length - 1) layerY++;
				else return layers[layerY].tile;
			}
		}
		return layers[layerY].tile;
	}

}
