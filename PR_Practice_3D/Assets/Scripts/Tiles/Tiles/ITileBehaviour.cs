using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PRTileArea.Events;

namespace PRTileArea {
	public interface ITileBehaviour {

		public void OnNeighborUpdate(TileUpdateEvent e);

		public bool OverrideMesh();

		public bool OverrideMatrix();

		public bool OverrideOcclusion();

		public Mesh GetMesh();

		public Quaternion GetRotation();

		public bool IsOccluding();

		public string ModifyID(string id);

	}
}
