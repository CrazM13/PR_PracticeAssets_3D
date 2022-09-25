using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInstance : MonoBehaviour {

	[SerializeField] private RuleTile rules;

	private Rule currentRule;

	public void OnNeighborUpdate(TileUpdateEvent e) {
		Rule rule = rules.GetRule(e.TileArea, e.ReceiverPosition);
		if (rule != null) {
			currentRule = rule;
		}
	}

	public string ID => rules.GetTileID();

	public Matrix4x4 GetMatrix() => Matrix4x4.TRS(transform.position, currentRule.GetRotation(), Vector3.one);

	public Mesh GetMesh() => currentRule.GetMesh();

	public Material GetMaterial() => rules.GetTileMaterial();

	public string GetRuleID() => $"{rules.GetTileID()}/{currentRule.GetRuleID()}";

	public void SetRuleTile(RuleTile ruleTile) {
		this.rules = ruleTile;
	}
}
