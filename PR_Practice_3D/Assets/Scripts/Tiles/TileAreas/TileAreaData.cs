using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileAreaData : ScriptableObject {

	[SerializeField] private TileData[] tileData;

	[System.Serializable]
	private class TileData {
		[SerializeField] public Vector3Int position;
		[SerializeField] public TileBase tile;

		public TileData(Vector3Int position, TileBase tile) {
			this.position = position;
			this.tile = tile;
		}
	}

	public void Save(TileArea tileArea) {
		List<TileInstance> tiles = tileArea.GetAllTiles();
		tileData = new TileData[tiles.Count];

		for (int i = 0; i < tileData.Length; i++) {
			tileData[i] = new TileData(tiles[i].GetTilePosition(), tiles[i].GetTile());
		}
	}

	public void Load(TileArea tileArea) {
		tileArea.ClearTileArea();

		for (int i = 0; i < tileData.Length; i++) {
			tileArea.SetTile(tileData[i].position, tileData[i].tile);
		}
	}

}
