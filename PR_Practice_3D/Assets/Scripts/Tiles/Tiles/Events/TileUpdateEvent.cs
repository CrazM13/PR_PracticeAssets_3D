using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea.Events {
	public class TileUpdateEvent {

		public TileArea TileArea { get; set; }
		public Vector3Int SourcePosition { get; set; }
		public Vector3Int ReceiverPosition { get; set; }

		public TileUpdateEvent(TileArea tileArea, Vector3Int source, Vector3Int receiver) {
			this.TileArea = tileArea;
			this.SourcePosition = source;
			this.ReceiverPosition = receiver;
		}

	}
}
