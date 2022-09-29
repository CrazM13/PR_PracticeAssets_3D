using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileAreaRenderer : MonoBehaviour {

	[SerializeField] private TileArea tileArea;

	private TileAreaRenderCache renderCache = new TileAreaRenderCache();

	void OnEnable() {
		tileArea = GetComponent<TileArea>();
	}

	void Update() {
		if (tileArea.IsDirty) {
			UpdateRenderArea();
			tileArea.IsDirty = false;
		}

		RenderNow();
	}

	private void UpdateRenderArea() {
		renderCache.ClearCache();

		List<TileInstance> tilesToRender = tileArea.GetAllTiles();
		if (tilesToRender.Count <= 0) return;

		tilesToRender.Sort((ti1, ti2) => ti1.GetTileID().CompareTo(ti2.GetTileID()));

		string currentBatch = tilesToRender[0].GetTileID();
		TileInstance firstOfBatch = tilesToRender[0];
		List<Matrix4x4> matrices = new List<Matrix4x4>();
		foreach (TileInstance tile in tilesToRender) {
			if (tile.IsOccluded()) continue;

			string newRuleID = tile.GetTileID();
			if (newRuleID == currentBatch && matrices.Count < 1023) {
				matrices.Add(Matrix4x4.TRS(tileArea.TileToWorld(tile.GetTilePosition()), tile.GetRotation(), Vector3.one));
			} else {
				renderCache.AddCache(firstOfBatch.GetMesh(), firstOfBatch.GetMaterial(), matrices.ToArray());
				firstOfBatch = tile;
				currentBatch = newRuleID;
				matrices.Clear();
				matrices.Add(Matrix4x4.TRS(tileArea.TileToWorld(tile.GetTilePosition()), tile.GetRotation(), Vector3.one));
			}

		}
		renderCache.AddCache(firstOfBatch.GetMesh(), firstOfBatch.GetMaterial(), matrices.ToArray());
	}

#if UNITY_EDITOR
	void OnDrawGizmos() {
		// Ensure continuous Update calls.
		if (!Application.isPlaying) {
			UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
			UnityEditor.SceneView.RepaintAll();
		}
#endif
	}

	public void ForceUpdate() {
		UpdateRenderArea();
	}

	public void RenderNow() {
		renderCache.Render();
	}

}
