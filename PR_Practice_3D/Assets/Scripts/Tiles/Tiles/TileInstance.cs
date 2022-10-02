using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PRTileArea.Events;

namespace PRTileArea {
	public class TileInstance {

		private TileBase tileBase;
		private ITileBehaviour tileBehaviour;

		private bool isOccluded;

		private Vector3Int tilePosition;

		public TileBase GetTile() => tileBase;

		public TileInstance(TileBase tile, Vector3Int tilePosition) {
			SetTile(tile);
			this.tilePosition = tilePosition;
		}

		public void OnNeighborUpdate(TileUpdateEvent e) {
			if (tileBehaviour != null) tileBehaviour.OnNeighborUpdate(e);

			CheckOcclusion(e.TileArea, e.ReceiverPosition);
		}

		public string ID => tileBase.GetTileID();

		public Quaternion GetRotation() {
			if (tileBehaviour?.OverrideMatrix() ?? false) return tileBehaviour.GetRotation();
			return Quaternion.identity;
		}

		public Vector3Int GetTilePosition() => tilePosition;

		public Mesh GetMesh() {
			if (tileBehaviour?.OverrideMesh() ?? false) return tileBehaviour.GetMesh();
			return tileBase.GetTileMesh();
		}

		public Material GetMaterial() => tileBase.GetTileMaterial();

		public string GetTileID() {
			string id = tileBase.GetTileID();
			if (tileBehaviour != null) return tileBehaviour.ModifyID(id);
			return id;
		}

		public bool CanOcclude() {
			if (tileBehaviour?.OverrideOcclusion() ?? false) return tileBehaviour.IsOccluding();
			return tileBase.IsOccluding();
		}

		public void SetTile(TileBase tileBase) {
			this.tileBase = tileBase;
			this.tileBehaviour = tileBase.GetBehaviour();
		}

		public void CheckOcclusion(TileArea tileArea, Vector3Int position) {
			isOccluded = true;

			if (!CheckTileOcclusion(tileArea, position + Vector3Int.up)) isOccluded = false;
			if (!CheckTileOcclusion(tileArea, position + Vector3Int.down)) isOccluded = false;

			if (!CheckTileOcclusion(tileArea, position + Vector3Int.left)) isOccluded = false;
			if (!CheckTileOcclusion(tileArea, position + Vector3Int.right)) isOccluded = false;

			if (!CheckTileOcclusion(tileArea, position + Vector3Int.forward)) isOccluded = false;
			if (!CheckTileOcclusion(tileArea, position + Vector3Int.back)) isOccluded = false;
		}

		private bool CheckTileOcclusion(TileArea tileArea, Vector3Int position) {
			if (!tileArea.TileExists(position)) {
				return false;
			} else {
				return tileArea.GetTileAt(position).CanOcclude();
			}
		}

		public bool IsOccluded() => isOccluded;
	}
}
