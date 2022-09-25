using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TileArea : MonoBehaviour {

	[Header("Edit Mode Settings")]
	[SerializeField] private bool isEditing;

	[SerializeField] private Vector3 tileSize;

	private Dictionary<Vector3Int, TileInstance> tiles = new Dictionary<Vector3Int, TileInstance>();

	void Update() {
		if (isEditing) {

			EditTileArea();

			
		}

		RenderArea();
	}

	private void EditTileArea() {

		

		UpdateEditedTiles();
	}

	public void UpdateEditedTiles() {
		List<Vector3Int> toRemove = new List<Vector3Int>();
		foreach (KeyValuePair<Vector3Int, TileInstance> tile in tiles) {
			if (!tile.Value) {
				toRemove.Add(tile.Key);
				continue;
			}

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

			Vector3Int tilePos = new Vector3Int(Mathf.FloorToInt(child.localPosition.x / tileSize.x), Mathf.FloorToInt(child.localPosition.y / tileSize.y), Mathf.FloorToInt(child.localPosition.z / tileSize.z));
			child.localPosition = new Vector3(tilePos.x * tileSize.x, tilePos.y * tileSize.y, tilePos.z * tileSize.z);

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

	public TileInstance GetTileAt(Vector3Int position) {
		if (tiles.ContainsKey(position)) return tiles[position];
		return null;
	}

	private void RenderArea() {
		if (tiles.Count <= 0) return;

		List<TileInstance> tilesToRender = new List<TileInstance>(tiles.Values);

		tilesToRender.Sort((ti1, ti2) => ti1.GetRuleID().CompareTo(ti2.GetRuleID()));

		string currentBatch = tilesToRender[0].GetRuleID();
		TileInstance firstOfBatch = tilesToRender[0];
		List<Matrix4x4> matrices = new List<Matrix4x4>();
		foreach (TileInstance tile in tilesToRender) {
			string newRuleID = tile.GetRuleID();
			if (newRuleID == currentBatch) {
				matrices.Add(tile.GetMatrix());
			} else {
				RenderCollection(firstOfBatch.GetMesh(), firstOfBatch.GetMaterial(), matrices.ToArray());
				firstOfBatch = tile;
				currentBatch = newRuleID;
				matrices.Clear();
				matrices.Add(tile.GetMatrix());
			}

		}
		RenderCollection(firstOfBatch.GetMesh(), firstOfBatch.GetMaterial(), matrices.ToArray());
	}

	private void RenderCollection(Mesh mesh, Material material, Matrix4x4[] matrices) {
		Graphics.DrawMeshInstanced(mesh, 0, material, matrices);
	}

	#region Raycast
	public bool Raycast(Vector3 position, Vector3 direction, float maxDistance, float stepDistance, out Vector3Int hit) {
		return Raycast(new Ray(position, direction), maxDistance, stepDistance, out hit);
	}

	public bool Raycast(Ray ray, float maxDistance, float stepDistance, out Vector3Int hit) {
		float distance = 0;
		while (distance < maxDistance) {
			Vector3 position = ray.GetPoint(distance);
			Vector3Int tilePos = WorldToTile(position);

			if (TileExists(tilePos)) {
				hit = tilePos;
				return true;
			}
			distance += stepDistance;
		}

		hit = default;
		return false;
	}
	#endregion

	public Vector3Int WorldToTile(Vector3 position) {
		position = transform.TransformPoint(position);

		return new Vector3Int(Mathf.RoundToInt(position.x / tileSize.x), Mathf.RoundToInt(position.y / tileSize.y), Mathf.RoundToInt(position.z / tileSize.z));
	}

	public Vector3 TileToWorld(Vector3Int position) {
		return new Vector3(position.x * tileSize.x, position.y * tileSize.y, position.z * tileSize.z);
	}

	public bool TileExists(Vector3Int position) {
		return tiles.ContainsKey(position);
	}

}
