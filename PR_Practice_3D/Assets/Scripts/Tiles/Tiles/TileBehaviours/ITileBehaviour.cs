using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileBehaviour {

	public void OnNeighborUpdate(TileUpdateEvent e);

	public bool OverrideMesh();

	public bool OverrideMatrix();

	public bool OverrideOcclusion();

	public Mesh GetMesh();

	public Matrix4x4 GetMatrix(Transform tileTransform);

	public bool IsOccluding();

	public string ModifyID(string id);

}
