using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea.RuleTile {
	[CreateAssetMenu(fileName = "New Rule Tile", menuName = "3D Tiles/Rule Tile", order = 0)]
	public class RuleTile : TileBase {

		[SerializeField] private Rule[] rules;

		private void OnEnable() {
			for (int i = 0; i < rules.Length; i++) {
				if (rules[i] == null || !rules[i].IsInitialized) rules[i] = new Rule();
			}
		}

		public Rule GetRule(TileArea tileArea, Vector3Int position) {

			Rule currentRule = null;

			for (int i = 0; i < rules.Length; i++) {
				if (rules[i].CanPassRule(tileArea, position)) currentRule = rules[i];
			}

			return currentRule;
		}

		public override ITileBehaviour GetBehaviour() {
			return new RuleTileBehaviour(this);
		}

	}
}
