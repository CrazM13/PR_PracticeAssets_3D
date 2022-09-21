using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileArea : MonoBehaviour {

	[Header("Edit Mode Settings")]
	[SerializeField] private bool isRecording;

	[SerializeField] private Vector3 tileSize;
	[SerializeField] private Material material;

	private Dictionary<Vector3Int, TileInstance> tiles = new Dictionary<Vector3Int, TileInstance>();

	void Start() {

	}

	void Update() {
		if (isRecording) {

			List<Vector3Int> toRemove = new List<Vector3Int>();
			foreach (KeyValuePair<Vector3Int, TileInstance> tile in tiles) {
				if (!tile.Value) tiles.Remove(tile.Key);

				Transform child = tile.Value.transform;
				Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(child.position.x / tileSize.x), Mathf.FloorToInt(child.position.y / tileSize.y), Mathf.FloorToInt(child.position.z / tileSize.z));
				if (tile.Key != tilePos) {
					toRemove.Add(tile.Key);
				}
			}

			foreach (Vector3Int position in toRemove) {
				tiles.Remove(position);
				MessageAdjacentTiles(position);
			}
			toRemove.Clear();

			for (int childIndex = 0; childIndex < transform.childCount; childIndex++) {
				Transform child = transform.GetChild(childIndex);

				Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(child.position.x / tileSize.x), Mathf.FloorToInt(child.position.y / tileSize.y), Mathf.FloorToInt(child.position.z / tileSize.z));
				child.position = new Vector3(tilePos.x * tileSize.x, tilePos.y * tileSize.y, tilePos.z * tileSize.z);
			
				if (tiles.ContainsKey(tilePos)) {
					if (tiles[tilePos] != child) {
						tiles[tilePos] = child.GetComponent<TileInstance>();
						MessageAdjacentTiles(tilePos);
					}
				} else {
					tiles.Add(tilePos, child.GetComponent<TileInstance>());
					MessageAdjacentTiles(tilePos);
				}
			
			}
		}
	}

	private void MessageAdjacentTiles(Vector3Int position) {
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				for (int z = -1; z <= 1; z++) {
					Vector3Int newPosition = new Vector3Int(position.x + x, position.y + y, position.z + z);
					
					if (tiles.ContainsKey(newPosition)) tiles[newPosition].OnNeighborUpdate(new TileUpdateEvent(this, position, newPosition));
				}
			}
		}
	}

	public bool CompareTiles(Vector3Int tile1, Vector3Int tile2) {
		bool containsTile1 = tiles.ContainsKey(tile1);
		bool containsTile2 = tiles.ContainsKey(tile2);
		if (containsTile1 && containsTile2) {
			return tiles[tile1].ID == tiles[tile2].ID;
		} else {
			return containsTile1 == containsTile2;
		}
	}

}
