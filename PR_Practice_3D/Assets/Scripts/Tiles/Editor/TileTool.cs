using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Tile Tool", typeof(TileArea))]
public class TileTool : EditorTool {

	#region Enum
	private enum Tools {
		NONE = 0,
		MOVE = 1,
		PAINT = 2,
		ERASE = 3
	}
	#endregion

	[SerializeField] private Texture2D mainToolIcon;
	[SerializeField] private Texture2D noneToolIcon;
	[SerializeField] private Texture2D moveToolIcon;
	[SerializeField] private Texture2D paintToolIcon;
	[SerializeField] private Texture2D eraseToolIcon;

	private Vector3Int? selectedTile;

	private Tools currentTool = Tools.NONE;

	private TileBase[] tiles;
	private TileBase paintingTile = null;

	public override GUIContent toolbarIcon =>
		new GUIContent() {
			image = mainToolIcon,
			text = "Tile Tool",
			tooltip = "Tile Tool"
		};

	public override void OnToolGUI(EditorWindow window) {

		if (!(window is SceneView sceneView)) return;

		DrawGUI();

		HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

		TileArea tileArea = target as TileArea;

		if (selectedTile.HasValue) {
			if (Event.current.button == 1) {
				selectedTile = null;
			}
		}
		ApplyActiveTool(tileArea);

		sceneView.Repaint();
	}

	#region GUI
	private void DrawGUI() {
		Handles.BeginGUI();
		DrawToolBar();
		if (currentTool == Tools.PAINT) DrawTileMenu();
		Handles.EndGUI();
	}

	private bool DrawButton(Texture2D icon, float size) {
		return GUILayout.Button(icon, GUILayout.Width(size), GUILayout.Height(size));
	}

	private void DrawToolBar() {
		using (new GUILayout.HorizontalScope()) {
			using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
				if (DrawButton(noneToolIcon, 64)) ChangeTool(Tools.NONE);
				if (DrawButton(moveToolIcon, 64)) ChangeTool(Tools.MOVE);
				if (DrawButton(paintToolIcon, 64)) ChangeTool(Tools.PAINT);
				if (DrawButton(eraseToolIcon, 64)) ChangeTool(Tools.ERASE);
			}
			GUILayout.FlexibleSpace();
		}
	}

	private void DrawTileMenu() {

		if (tiles == null || tiles.Length <= 0) tiles = GetAllTiles();
		else {
			for (int i = 0; i < tiles.Length; i++) {
				GUILayout.BeginArea(new Rect(Screen.width - 130, 32 + (34 * i), 120, 32));
				if (DrawButton(paintToolIcon, 32)) paintingTile = tiles[i];
				GUILayout.EndArea();
			}
		}
	}
	#endregion

	#region Tools
	private void ApplyActiveTool(TileArea tileArea) {
		switch (currentTool) {
			case Tools.NONE:
				SelectTile(tileArea);
				break;
			case Tools.MOVE:
				MoveTiles(tileArea);
				break;
			case Tools.PAINT:
				PaintTool(tileArea);
				break;
			case Tools.ERASE:
				EraseTool(tileArea);
				break;
		}
		
	}

	private void MoveTiles(TileArea tileArea) {
		if (selectedTile.HasValue) {
			Vector3 newPosition = Handles.PositionHandle(tileArea.TileToWorld(selectedTile.Value), Quaternion.identity);
			Vector3Int newTilePos = tileArea.WorldToTile(newPosition);
			if (selectedTile.Value != newTilePos && !tileArea.TileExists(newTilePos)) {
				tileArea.SetTile(newTilePos, tileArea.GetTileAt(selectedTile.Value).GetTile());
				tileArea.SetTile(selectedTile.Value, null);

				tileArea.IsDirty = true;
				selectedTile = newTilePos;
			}
		} else {
			SelectTile(tileArea);
		}
	}

	private void SelectTile(TileArea tileArea) {
		Vector2 mousePos = Event.current.mousePosition;
		Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

		if (tileArea.Raycast(ray, 100, 0.1f, out TileRaycastHit hit)) {
			Handles.DrawWireCube(hit.worldPosition, tileArea.TileSize);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
				selectedTile = hit.position;
			}
		}
	}

	private void SelectEmptyTile(TileArea tileArea) {
		Vector2 mousePos = Event.current.mousePosition;
		Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

		if (tileArea.Raycast(ray, 100, 0.1f, out TileRaycastHit hit)) {
			Vector3Int prevTile = hit.position + Vector3Int.RoundToInt(hit.normal);
			Vector3 hitPosition = tileArea.TileToWorld(prevTile);

			Handles.DrawWireCube(hitPosition, tileArea.TileSize);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
				selectedTile = prevTile;
			}
		}
	}

	private void PaintTool(TileArea tileArea) {
		selectedTile = null;
		if (paintingTile) {
			SelectEmptyTile(tileArea);
			if (selectedTile.HasValue) {
				tileArea.SetTile(selectedTile.Value, paintingTile);
			}
		}
	}

	private void EraseTool(TileArea tileArea) {
		selectedTile = null;
		SelectTile(tileArea);
		if (selectedTile.HasValue) {
			tileArea.SetTile(selectedTile.Value, null);
		}
	}

	private void ChangeTool(Tools newTool) {
		if (newTool != currentTool) {
			currentTool = newTool;
			selectedTile = null;
		}
	}
	#endregion

	private TileBase[] GetAllTiles() {
		string[] guids = AssetDatabase.FindAssets($"t:{typeof(TileBase)}");
		TileBase[] tiles = new TileBase[guids.Length];

		for (int i = 0; i < tiles.Length; i++) {
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			tiles[i] = AssetDatabase.LoadAssetAtPath<TileBase>(path);
		}

		return tiles;
	}

}
