using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHeightArea : TileArea {

	[Header("Height Map")]
	[SerializeField] private Texture2D heightMap;

	[Header("Landscape")]
	//[SerializeField] private float horizontalScale;

	[Header("Settings")]
	[SerializeField] private float horizontalScale;
	[SerializeField] private float verticalScale;

	private void Start() {

		int textureWidth = heightMap.width;
		int textureHeight = heightMap.height;

		int mapWidth = Mathf.FloorToInt(textureWidth * horizontalScale);
		int mapDepth = Mathf.FloorToInt(textureHeight * horizontalScale);

		for (int x = -mapWidth / 2; x < mapWidth / 2; x++) {
			for (int z = -mapDepth / 2; z < mapDepth / 2; z++) {

				int textureCoordX = Mathf.RoundToInt(x / horizontalScale);
				int textureCoordZ = Mathf.RoundToInt(z / horizontalScale);

				int height = Mathf.RoundToInt(heightMap.GetPixel(textureCoordX, textureCoordZ).r * verticalScale);
				
				for (int y = 0; y < height; y++) {

				}
			}
		}

	}

}
