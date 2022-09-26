using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInstance : MonoBehaviour {

	[SerializeField] private TileBase tileBase;
	private ITileBehaviour tileBehaviour;

	private bool isOccluded;

	public void OnNeighborUpdate(TileUpdateEvent e) {
		if (tileBehaviour != null) tileBehaviour.OnNeighborUpdate(e);
		else if (tileBase) tileBehaviour = tileBase.GetBehaviour();

		CheckOcclusion(e.TileArea, e.ReceiverPosition);
	}

	public string ID => tileBase.GetTileID();

	public Matrix4x4 GetMatrix() {
		if (tileBehaviour?.OverrideMatrix() ?? false) return tileBehaviour.GetMatrix(transform);
		return Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
	}

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
		for (int y = -1; y <= 1; y++) {
			for (int x = -1; x <= 1; x++) {
				for (int z = -1; z <= 1; z++) {
					if (x == 0 && y == 0 && z == 0) continue;

					Vector3Int newPosition = position + new Vector3Int(x, y, z);
					if (!tileArea.TileExists(newPosition)) {
						isOccluded = false;
						break;
					} else {
						if (!tileArea.GetTileAt(newPosition).CanOcclude()) {
							isOccluded = false;
							break;
						}
					}
				}
			}
		}
	}

	public bool IsOccluded() => isOccluded;
}
