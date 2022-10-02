using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea {
	[CreateAssetMenu(fileName = "New Tile", menuName = "3D Tiles/Tile", order = 0)]
	public class TileBase : ScriptableObject {

		[SerializeField] private string tileID;
		[SerializeField] private Texture2D tileThumbnail;
		[SerializeField] private Material tileMaterial;
		[SerializeField] private Mesh tileMesh;
		[SerializeField] private bool occluding;

		public Material GetTileMaterial() => tileMaterial;
		public Mesh GetTileMesh() => tileMesh;

		public string GetTileID() => tileID;

		public bool IsOccluding() => occluding;

		public virtual ITileBehaviour GetBehaviour() => null;

		public Texture2D GetThumbnail() {
			return tileThumbnail;
		}

	}
}
