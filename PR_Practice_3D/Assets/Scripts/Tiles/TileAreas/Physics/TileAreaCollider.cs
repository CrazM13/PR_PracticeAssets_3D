using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea.Physics {
	[RequireComponent(typeof(MeshCollider))]
	public class TileAreaCollider : MonoBehaviour {

		[SerializeField] private TileArea tileArea;

		private Mesh cachedMesh;

		[ContextMenu("Update TileArea Collider")]
		public void UpdateMesh() {
			cachedMesh = new Mesh();

			List<TileInstance> tiles = tileArea.GetAllTiles();
			
		}

	}
}
