using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea {
	public class TileAreaBuilderHeightmap : MonoBehaviour {

		[Header("Tile Area")]
		[SerializeField] private TileArea tileArea;

		[Header("Height Map")]
		[SerializeField] private Texture2D heightMap;

		[Header("Landscape")]
		[SerializeField] private TileLandscape landscape;

		[Header("Settings")]
		[SerializeField] private float horizontalScale;
		[SerializeField] private float verticalScale;



		[ContextMenu("Build From Heightmap")]
		private void CreateTileAreaFromHeightmap() {
			int mapWidth = Mathf.FloorToInt(heightMap.width * horizontalScale);
			int mapDepth = Mathf.FloorToInt(heightMap.height * horizontalScale);

			for (int x = 0; x < mapWidth; x++) {
				for (int z = 0; z < mapDepth; z++) {

					int textureCoordX = Mathf.RoundToInt(x / horizontalScale);
					int textureCoordZ = Mathf.RoundToInt(z / horizontalScale);

					int height = Mathf.RoundToInt(heightMap.GetPixel(textureCoordX, textureCoordZ).r * verticalScale);

					for (int y = 0; y <= height; y++) {
						TileBase tile = landscape.GetTileByY(y);
						tileArea.SetTile(new Vector3Int(x - (mapWidth / 2), y, z - (mapDepth / 2)), tile, false);
					}
				}
			}

			tileArea.NotifyAllTiles();
		}

	}
}
