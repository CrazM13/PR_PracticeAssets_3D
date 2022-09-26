using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleTileBehaviour : ITileBehaviour {

	private RuleTile rules;
	private Rule currentRule;

	public RuleTileBehaviour(RuleTile rules) {
		this.rules = rules;
		this.currentRule = null;
	}

	public Matrix4x4 GetMatrix(Transform tileTransform) {
		return Matrix4x4.TRS(tileTransform.position, currentRule.GetRotation(), Vector3.one);
	}

	public Mesh GetMesh() {
		return currentRule.GetMesh();
	}

	public void OnNeighborUpdate(TileUpdateEvent e) {
		Rule rule = rules.GetRule(e.TileArea, e.ReceiverPosition);
		if (rule != null) {
			currentRule = rule;
		}
	}

	public bool OverrideMatrix() {
		return currentRule != null;
	}

	public bool OverrideMesh() {
		return currentRule != null;
	}

	public string ModifyID(string id) {
		if (currentRule != null) return $"{id}/{currentRule.GetRuleID()}";
		return id;
	}

	public bool OverrideOcclusion() {
		return currentRule != null;
	}

	public bool IsOccluding() {
		return currentRule.IsOccluding();
	}
}
