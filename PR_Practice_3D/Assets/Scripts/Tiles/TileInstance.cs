using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInstance : MonoBehaviour {

	[SerializeField] private string id;
	[SerializeField] private new MeshFilter renderer;
	[SerializeField] private RuleTile rules;

	public void OnNeighborUpdate(TileUpdateEvent e) {
		Rule rule = rules.GetRule(e.TileArea, e.ReceiverPosition);
		if (rule != null) {
			renderer.mesh = rule.GetMesh();
		}
	}

	public string ID => id;
}
