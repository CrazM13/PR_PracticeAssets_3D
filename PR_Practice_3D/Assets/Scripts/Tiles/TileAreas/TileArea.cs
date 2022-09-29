using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TileArea : MonoBehaviour {

	public UnityEvent<TileUpdateEvent> OnTileUpdated { get; private set; } = new UnityEvent<TileUpdateEvent>();

	[SerializeField] private Vector3 tileSize;

	private Dictionary<Vector3Int, TileInstance> tiles = new Dictionary<Vector3Int, TileInstance>();

	public Vector3 TileSize => tileSize;

	public bool IsDirty { get; set; } = true;

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

	#region Raycast
	public bool Raycast(Vector3 position, Vector3 direction, float maxDistance, float stepDistance, out TileRaycastHit hit) {
		return Raycast(new Ray(position, direction), maxDistance, stepDistance, out hit);
	}

	public bool Raycast(Ray ray, float maxDistance, float stepDistance, out TileRaycastHit hit) {
		float distance = 0;
		Vector3Int previousTile = WorldToTile(ray.origin);
		while (distance < maxDistance) {
			Vector3 position = ray.GetPoint(distance);
			Vector3Int tilePos = WorldToTile(position);

			if (TileExists(tilePos)) {
				hit = new TileRaycastHit() {
					position = tilePos,
					worldPosition = TileToWorld(tilePos),
					normal = SimplifyDirection(previousTile - tilePos)
				};
				return true;
			}

			previousTile = tilePos;
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

	public void SetTile(Vector3Int position, TileBase tile, bool notifyNeighbors = true) {
		if (tiles.ContainsKey(position)) {
			if (tile != null) tiles[position].SetTile(tile);
			else tiles.Remove(position);
		} else if (tile != null) {
			TileInstance ti = new TileInstance(tile, position);

			tiles.Add(position, ti);
		}

		IsDirty = true;
		OnTileUpdated.Invoke(new TileUpdateEvent(this, position, position));
		if (notifyNeighbors) MessageAdjacentTiles(position);

#if UNITY_EDITOR
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif
	}

	public void NotifyAllTiles() {
		foreach (TileInstance tile in tiles.Values) {
			TileUpdateEvent @event = new TileUpdateEvent(this, tile.GetTilePosition(), tile.GetTilePosition());
			OnTileUpdated.Invoke(@event);
			tile.OnNeighborUpdate(@event);
		}
	}

	public List<TileInstance> GetAllTiles() => new List<TileInstance>(tiles.Values);

	[ContextMenu("Clear Tile Area")]
	public void ClearTileArea() {
		tiles.Clear();
		IsDirty = true;
	}

	private Vector3 SimplifyDirection(Vector3 direction) {
		float x = Mathf.Abs(direction.x);
		float y = Mathf.Abs(direction.y);
		float z = Mathf.Abs(direction.z);

		if (x > y) {
			if (x > z) {
				return new Vector3(Mathf.Sign(direction.x) * 1, 0 , 0);
			} else {
				return new Vector3(0, 0, Mathf.Sign(direction.z) * 1);
			}
		} else {
			if (y > z) {
				return new Vector3(0, Mathf.Sign(direction.y) * 1, 0);
			} else {
				return new Vector3(0, 0, Mathf.Sign(direction.z) * 1);
			}
		}

	}

#if UNITY_EDITOR
	
	private void Save(Scene _) {
		string path = $"Assets/TileAreaData/{SceneManager.GetActiveScene().name}/{name}/data.asset";
		TileAreaData dataObject = AssetDatabase.LoadAssetAtPath<TileAreaData>(path);
		if (!dataObject) {
			dataObject = ScriptableObject.CreateInstance<TileAreaData>();
			CreatePath(path);
			AssetDatabase.CreateAsset(dataObject, path);
		}
		dataObject.Save(this);
		EditorUtility.SetDirty(dataObject);
	}

	private void Load() {
		string path = $"Assets/TileAreaData/{SceneManager.GetActiveScene().name}/{name}/data.asset";
		Debug.Log($"Loading {path}");
		TileAreaData dataObject = AssetDatabase.LoadAssetAtPath<TileAreaData>(path);
		if (dataObject) {
			dataObject.Load(this);
		}
	}

	private void CreatePath(string path) {
		string[] paths = path.Split('/');
		string newPath = paths[0];
		for (int i = 1; i < paths.Length; i++) {
			if (!AssetDatabase.IsValidFolder($"{newPath}/{paths[i]}")) {
				AssetDatabase.CreateFolder(newPath, paths[i]);
				newPath += "/" + paths[i];
			}
			
		}
	}

	private void OnEnable() {
		EditorSceneManager.sceneSaved -= Save;
		EditorSceneManager.sceneSaved += Save;
		Load();
	}
#endif
}
