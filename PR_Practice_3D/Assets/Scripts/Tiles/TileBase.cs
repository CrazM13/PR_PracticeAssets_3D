using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : ScriptableObject {

	[SerializeField] private string tileID;
	[SerializeField] private Material tileMaterial;

	public Material GetTileMaterial() => tileMaterial;

	public string GetTileID() => tileID;

}
