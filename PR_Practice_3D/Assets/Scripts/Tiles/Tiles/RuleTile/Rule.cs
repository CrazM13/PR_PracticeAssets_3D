using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea.RuleTile {
	[System.Serializable]
	public class Rule {

		[SerializeField] private RuleAreaTypes[] ruleArea = new RuleAreaTypes[27];
		[SerializeField] private string ruleID;
		[SerializeField] private Mesh mesh;
		[SerializeField] private Vector3 meshRotation;
		[SerializeField] private bool occluding;

		public Rule() {
			for (int i = 0; i < ruleArea.Length; i++) ruleArea[i] = RuleAreaTypes.NONE;
		}

		public bool CanPassRule(TileArea tileArea, Vector3Int position) {
			bool passed = true;

			int index = 0;
			for (int y = 1; y >= -1; y--) {
				for (int x = -1; x <= 1; x++) {
					for (int z = 1; z >= -1; z--) {
						if (x != 0 || y != 0 || z != 0) {
							bool tilesMatch = tileArea.CompareTiles(position, position + new Vector3Int(x, y, z));

							if (ruleArea[index] == RuleAreaTypes.MATCH) {
								if (!tilesMatch) {
									passed = false;
								}
							} else if (ruleArea[index] == RuleAreaTypes.NO_MATCH) {
								if (tilesMatch) {
									passed = false;
								}
							}
						}

						index++;
					}
				}
			}

			return passed;
		}

		public Mesh GetMesh() => mesh;
		public Matrix4x4 GetMatrix() => Matrix4x4.TRS(Vector3.zero, GetRotation(), Vector3.one);
		public Quaternion GetRotation() => Quaternion.Euler(meshRotation);

		public bool IsOccluding() => occluding;

		public string GetRuleID() => ruleID;

		public bool IsInitialized => ruleArea.Length == 27;

	}
}
