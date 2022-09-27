using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Tile Tool", typeof(TileArea))]
public class TileTool : EditorTool {
	[SerializeField]
	private Texture2D icon;

	private Vector3Int? selectedTile;

	public override GUIContent toolbarIcon =>
		new GUIContent() {
			image = icon,
			text = "Tile Tool",
			tooltip = "Tile Tool"
		};

	public override void OnToolGUI(EditorWindow window) {

		if (!(window is SceneView sceneView)) return;

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		Event e = Event.current;

		Vector2 mousePos = e.mousePosition;

		Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
		TileArea tileArea = target as TileArea;

		if (tileArea.Raycast(ray, 100, 0.1f, out Vector3Int hit)) {
			Vector3 hitPosition = tileArea.TileToWorld(hit);
			Handles.DrawWireCube(hitPosition, tileArea.TileSize);
			if (e.type == EventType.MouseDown) {
				selectedTile = hit;
			}
		}

		if (selectedTile.HasValue) {
			Vector3 newPosition = Handles.PositionHandle(tileArea.TileToWorld(selectedTile.Value), Quaternion.identity);
			Vector3Int newTilePos = tileArea.WorldToTile(newPosition);
			if (selectedTile.Value != newTilePos && !tileArea.TileExists(newTilePos)) {
				tileArea.SetTile(newTilePos, tileArea.GetTileAt(selectedTile.Value).GetTile());
				tileArea.SetTile(selectedTile.Value, null);
				tileArea.ForceUpdateRenderers();
				selectedTile = newTilePos;
			}
		}
		sceneView.Repaint();
	}

}
