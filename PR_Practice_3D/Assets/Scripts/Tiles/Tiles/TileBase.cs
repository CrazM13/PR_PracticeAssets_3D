using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "3D Tiles/Tile", order = 0)]
public class TileBase : ScriptableObject {

	[SerializeField] private string tileID;
	[SerializeField] private Material tileMaterial;
	[SerializeField] private Mesh tileMesh;
	[SerializeField] private bool occluding;

	public Material GetTileMaterial() => tileMaterial;
	public Mesh GetTileMesh() => tileMesh;

	public string GetTileID() => tileID;

	public bool IsOccluding() => occluding;

	public virtual ITileBehaviour GetBehaviour() => null;

}
